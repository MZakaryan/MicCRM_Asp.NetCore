using MicCRM.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicCRM.Helpers
{
    public static class Utilities
    {
        public static IEnumerable<SelectListItem> GetSelectListItem<TEntity>(IEnumerable<TEntity> tEntity)
            where TEntity : EntityBase
        {
            List<SelectListItem> tEntityToSelect = new List<SelectListItem>();

            foreach (TEntity item in tEntity)
            {
                tEntityToSelect.Add(new SelectListItem()
                {
                    Value = item.Id.ToString(),
                    Text = item.ToString()
                });
            }
            return tEntityToSelect;
        }

        public static string RemoveWhiteSpace(this string str)
        {
            return new string(str.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        public static DateTime UNIXTimeToDateTime(this string unixTimeString)
        {

            char[] ch = new char[] {'.'};

            string unixTimeStr = unixTimeString.Split(ch)[0];

            double unixTime = double.Parse(unixTimeStr);

            DateTime dateTime = new DateTime(1899, 12, 30)
                .AddDays(unixTime)
                .ToLocalTime();

            return dateTime;
        }

    }
}
