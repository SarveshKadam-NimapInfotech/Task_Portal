using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Task_Portal.API
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileUploadParams = context.MethodInfo.GetParameters()
           .Where(p => p.ParameterType == typeof(IFormFile))
           .ToList();

            if (!fileUploadParams.Any())
            {
                return;
            }

            foreach (var param in fileUploadParams)
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = new Dictionary<string, OpenApiSchema>
                                {
                                    [param.Name] = new OpenApiSchema
                                    {
                                        Type = "string",
                                        Format = "binary"
                                    }
                                },
                                Required = new HashSet<string> { param.Name }
                            }
                        }
                    }
                };
            }
        }
    }
}
