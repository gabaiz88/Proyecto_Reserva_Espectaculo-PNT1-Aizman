﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Reserva_Espectaculo.Models;
using System.Collections.Generic;
using System.Linq;

namespace Reserva_Espectaculo.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static IEnumerable<SelectListItem> GetEnumSelectListWithDefaultValue<TEnum>(this IHtmlHelper htmlHelper, TEnum? defaultValue)
            where TEnum : struct
        {
            var selectList = htmlHelper.GetEnumSelectList<TEnum>().ToList();
            selectList.Single(x => x.Value == $"{(int)(object)defaultValue}").Selected = true;
            return selectList;
        }
    }

}