using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text.Encodings.Web;

namespace VideoSharing.CustomMiddleware;

public class AntiXssMiddleware
{
    private readonly RequestDelegate _next;

    public AntiXssMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.ContentType != null)
        {
            bool containsXss = false;
            if (context.Request.ContentType.Contains("multipart/form-data"))
            {
                var formBody = await context.Request.ReadFormAsync();
                var keyValuePairs = formBody.ToDictionary(x => x.Key, x => x.Value.ToString());
                containsXss = ValidateContent(keyValuePairs);
            }
            else if (MediaTypeNames.Application.Json.Equals(context.Request.ContentType))
            {
                var keyValuePairs = await context.Request.ReadFromJsonAsync<Dictionary<string, string>>();
                containsXss = ValidateContent(keyValuePairs);
            }
            if (containsXss)
            {
                var problemDetail = new ProblemDetails
                {
                    Title = "XSS Has been detected in the request",
                    Detail = "Please refrain from using scripts in the request",
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://owasp.org/www-community/attacks/xss/",
                    Instance = "/videos"
                };
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(problemDetail));
                return;
            }
        }
        await _next(context);
    }

    private static bool ValidateContent(Dictionary<string, string> keyValuePairs)
    {
        var jsEncoder = JavaScriptEncoder.Default;
        foreach (var item in keyValuePairs)
        {
            var sanitised = jsEncoder.Encode(item.Value);
            if (sanitised != item.Value)
            {
                return true;
            }
        };
        return false;
    }
}