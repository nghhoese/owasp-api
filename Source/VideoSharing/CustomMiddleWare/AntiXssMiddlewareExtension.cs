namespace VideoSharing.CustomMiddleware;

public static class AntiXssMiddlewareExtension
{
    public static IApplicationBuilder UseAntiXssMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AntiXssMiddleware>();
    }
}