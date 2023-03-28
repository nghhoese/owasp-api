namespace VideoSharing.CustomMiddleware;

public static class BlockHttpMiddlewareExtension
{
    public static IApplicationBuilder UseBlockHttpMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BlockHttpMiddleware>();
    }
}