using System;
using System.Globalization;
using System.Web.Mvc;

namespace NeuronExportedDocuments.Infrastructure
{
    public class DateTimeModelBinder : DefaultModelBinder
    {

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            return DateTime.Parse(value.AttemptedValue, CultureInfo.GetCultureInfo("ru-RU"));

        }
    }
}