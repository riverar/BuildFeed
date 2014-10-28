using System;
using System.Globalization;
using System.Web.Mvc;

namespace BuildFeed
{
    public class BuildDateTimeModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            DateTime retValue;
            bool success = DateTime.TryParseExact(value.AttemptedValue, "yyMMdd-HHmm", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out retValue);
            return success ? retValue as DateTime? : null as DateTime?;
        }
    }
}
