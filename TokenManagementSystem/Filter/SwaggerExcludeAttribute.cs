using System;
using System.Diagnostics.CodeAnalysis;

namespace TokenManagementSystem.Filter
{
    [AttributeUsage(AttributeTargets.Property)]
    [ExcludeFromCodeCoverage]
    public class SwaggerExcludeAttribute : Attribute { }
}
