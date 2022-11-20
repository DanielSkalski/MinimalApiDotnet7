namespace WebApplication2;

public static class Filters
{
    internal static EndpointFilterDelegate RequestAuditor(EndpointFilterFactoryContext context, EndpointFilterDelegate next)
    {
        var loggerFactory = context.ApplicationServices.GetService<ILoggerFactory>()!;
        var logger = loggerFactory.CreateLogger(nameof(RequestAuditor));
        return (invocationContext) =>
        {
            logger.LogInformation($"Received a request for {invocationContext.HttpContext.Request.Path}");
            return next(invocationContext);
        };
    }
}
