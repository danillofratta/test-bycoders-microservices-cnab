using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cnab.Api.Presentation.Swagger;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Title = $"api-cnab {description.ApiVersion}",
                Version = description.GroupName,
                Description = "API para upload e consulta de operações CNAB"
            });
        }

        options.DocInclusionPredicate((docName, apiDesc) =>
        {
            if (!apiDesc.TryGetMethodInfo(out var methodInfo))
                return false;

            var versions = apiDesc.CustomAttributes()
                .Where(attr => attr.GetType().Name == "MapToApiVersionAttribute" || attr.GetType().Name == "ApiVersionAttribute")
                .ToList();

            if (!versions.Any()) return true;
            return true;
        });

    }
}
