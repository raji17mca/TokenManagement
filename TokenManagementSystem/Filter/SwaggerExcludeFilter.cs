﻿using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TokenManagementSystem.Filter
{
    [ExcludeFromCodeCoverage]
    public class SwaggerExcludeFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
                if (schema?.Properties == null)
                {
                    return;
                }

                var excludedProperties =
                    context.Type.GetProperties().Where(
                        t => t.GetCustomAttribute<SwaggerExcludeAttribute>() != null);

                foreach (var excludedProperty in excludedProperties)
                {
                    var propertyToRemove =
                        schema.Properties.Keys.SingleOrDefault(
                            x => x.ToLower() == excludedProperty.Name.ToLower());

                    if (propertyToRemove != null)
                    {
                        schema.Properties.Remove(propertyToRemove);
                    }
                }
            }
        }
}
