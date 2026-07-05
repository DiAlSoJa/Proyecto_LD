namespace LD.Api.Middlewares
{
    using System.Security.Claims;
    using Serilog.Context;

    /// <summary>
    /// Empuja propiedades de contexto (usuario, origen, request) al <see cref="LogContext"/>
    /// para que el sink de sys_logs las mapee a sus columnas. Se registra DESPUÉS de
    /// autenticación (para tener <c>context.User</c>) y ANTES de UseSerilogRequestLogging
    /// (para que el evento de fin de request vea estas propiedades vigentes).
    ///
    /// UserId/UserName quedan NULL en requests anónimos (login, health, etc.).
    /// StatusCode lo aporta UseSerilogRequestLogging automáticamente.
    /// </summary>
    public class SysLogEnrichmentMiddleware
    {
        private readonly RequestDelegate _next;

        public SysLogEnrichmentMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var user = context.User;
            var isAuthenticated = user?.Identity?.IsAuthenticated == true;

            var userId = isAuthenticated ? user!.FindFirst(ClaimTypes.NameIdentifier)?.Value : null;
            var userName = isAuthenticated ? user!.Identity!.Name : null;
            var sourceApp = context.Request.Headers["X-Client-App"].FirstOrDefault();

            int? warehouseId =
                int.TryParse(context.Request.Headers["X-Warehouse-Id"].FirstOrDefault(), out var w)
                    ? w
                    : null;

            using (LogContext.PushProperty("UserId", userId))
            using (LogContext.PushProperty("UserName", userName))
            using (LogContext.PushProperty("SourceApp", sourceApp))
            using (LogContext.PushProperty("WarehouseId", warehouseId))
            using (LogContext.PushProperty("RequestPath", context.Request.Path.Value))
            using (LogContext.PushProperty("RequestMethod", context.Request.Method))
            using (LogContext.PushProperty("ClientIp", context.Connection.RemoteIpAddress?.ToString()))
            using (LogContext.PushProperty("MachineName", Environment.MachineName))
            using (LogContext.PushProperty("CorrelationId", context.TraceIdentifier))
            {
                await _next(context);
            }
        }
    }
}
