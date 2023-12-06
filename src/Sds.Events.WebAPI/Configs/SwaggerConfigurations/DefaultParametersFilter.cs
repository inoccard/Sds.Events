using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IATec.DocSearch.Api.SwaggerConfigurations
{
    public class DefaultParametersFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            if (parameter == null)
                return;

            if (parameter is OpenApiParameter nonBodyParameter)
            {
                nonBodyParameter.Description ??= context.ApiParameterDescription.ModelMetadata.Description;

                if (context.ApiParameterDescription.RouteInfo != null)
                {
                    parameter.Required |= !context.ApiParameterDescription.RouteInfo.IsOptional;
                }
            }
        }
    }
}