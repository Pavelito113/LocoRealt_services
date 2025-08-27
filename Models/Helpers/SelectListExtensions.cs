using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace LocoRealt.Models.Helpers
{
    public static class SelectListExtensions
    {
        public static SelectList ToSelectList<T>(
            this IEnumerable<T> items,
            string valueField,
            string textField,
            object? selectedValue = null)
        {
            return new SelectList(
                items ?? [],
                valueField,
                textField,
                selectedValue);
        }
    }
}
