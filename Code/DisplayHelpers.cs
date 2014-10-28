using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BuildFeed
{
    public static class DisplayHelpers
    {
        public static string GetDisplayTextForEnum(object o)
        {
            var result = null as string;
            var display = o.GetType()
                           .GetMember(o.ToString()).First()
                           .GetCustomAttributes(false)
                           .OfType<DisplayAttribute>()
                           .LastOrDefault();

            if (display != null)
            {
                result = display.GetName();
            }

            return result ?? o.ToString();
        }
    }
}