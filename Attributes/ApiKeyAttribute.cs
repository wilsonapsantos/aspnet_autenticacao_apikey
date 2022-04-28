using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace aspnet_autenticacao_apikey.Attributes;

[AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    private const string ApiKeyName = "ApiKey";

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        StringValues keyName = context.HttpContext.Request.Headers[ApiKeyName];

        if (string.IsNullOrEmpty(keyName))
        {
            context.Result = new ContentResult()
            {
                StatusCode = 401,
                Content = "ApiKey não encontrada"
            };
            return;
        }

        var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

        var apiKey = appSettings.GetSection("ApiKey:Clients");
        bool apiKeyValid = false;

        foreach (IConfigurationSection section in apiKey.GetChildren())
        {
            var value = section.GetValue<string>("Value");
            if (keyName == value)
            {
                apiKeyValid = true;
                break;
            }
        }

        if (!apiKeyValid)
        {
            context.Result = new ContentResult()
            {
                StatusCode = 403,
                Content = "Acesso não autorizado"
            };
            return;
        }

        await next();
    }
}