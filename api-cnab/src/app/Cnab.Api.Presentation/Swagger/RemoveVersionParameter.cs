using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cnab.Api.Presentation.Swagger;

public class RemoveVersionParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null) return;

        var versionParameter = operation.Parameters.FirstOrDefault(p => p.Name == "version");
        if (versionParameter != null)
        {
            operation.Parameters.Remove(versionParameter);
        }
    }
}
