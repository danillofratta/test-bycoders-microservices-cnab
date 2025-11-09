using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var relativePath = context.ApiDescription.RelativePath?.TrimEnd('/');
        var httpMethod = context.ApiDescription.HttpMethod?.ToUpperInvariant();

        // Apply to any POST endpoint with 'upload' in the path
        if (string.IsNullOrEmpty(relativePath) || !string.Equals(httpMethod, "POST", StringComparison.OrdinalIgnoreCase))
            return;

        if (!relativePath.Contains("upload", StringComparison.OrdinalIgnoreCase))
            return;

        // If already has multipart/form-data described, do nothing
        if (operation.RequestBody?.Content?.ContainsKey("multipart/form-data") == true)
            return;

        operation.RequestBody = new OpenApiRequestBody
        {
            Content =
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties =
                        {
                            ["file"] = new OpenApiSchema { Type = "string", Format = "binary" }
                        },
                        Required = new HashSet<string> { "file" }
                    }
                }
            }
        };
    }
}
