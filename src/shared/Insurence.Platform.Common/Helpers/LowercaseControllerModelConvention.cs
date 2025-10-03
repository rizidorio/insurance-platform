using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Insurence.Platform.Common.Helpers;

public class LowercaseControllerModelConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        var originalTemplate = controller.Selectors
            .FirstOrDefault(s => s.AttributeRouteModel != null)?.AttributeRouteModel?.Template;

        if (!string.IsNullOrEmpty(originalTemplate))
        {
            var newTemplate = originalTemplate.Replace("[controller]", controller.ControllerName.ToLowerInvariant());

            foreach (var selector in controller.Selectors.Where(s => s.AttributeRouteModel != null))
            {
                selector.AttributeRouteModel!.Template = newTemplate;
            }
        }
    }
}
