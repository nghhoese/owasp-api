namespace VideoSharing.CustomMiddleware;

public class BlockHttpMiddleware
{
    private readonly RequestDelegate _next;

    public BlockHttpMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.IsHttps)
        {
            await _next(context);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}