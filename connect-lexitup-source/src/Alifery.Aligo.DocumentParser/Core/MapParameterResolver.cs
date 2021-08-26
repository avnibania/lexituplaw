using Alifery.Aligo.DocumentParser.Models;
using Alifery.Connect.CoreEntities.AligoEntities.DocumentParser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alifery.Aligo.DocumentParser.Core
{
    public static class MapParameterResolver
    {

        public static string GetStringParameterValue(this ICollection<MapParametersDto> parameters, ParameterKeys key)
        {
            return parameters.FirstOrDefault(p => p.Key == key.ToString())?.Value ?? string.Empty;
        }

        public static string GetStringParameterValue(this ICollection<PostProcessMapParametersDto> parameters, ParameterKeys key)
        {
            return parameters.FirstOrDefault(p => p.Key == key.ToString())?.Value ?? string.Empty;
        }

        public static int GetNumericParameterValue(this ICollection<MapParametersDto> parameters, ParameterKeys key)
        {
            var item = parameters.FirstOrDefault(p => p.Key == key.ToString());
            return Convert.ToInt32(item?.Value);
        }

        public static int GetPageNo(this ICollection<MapParametersDto> parameters)
        {
            var item = parameters.FirstOrDefault(p => p.Key == ParameterKeys.PageNo.ToString());
            return Convert.ToInt32(item.Value) - 1;
        }

        public static int GetLineNo(this ICollection<MapParametersDto> parameters)
        {
            var item = parameters.FirstOrDefault(p => p.Key == ParameterKeys.LineNo.ToString());
            return Convert.ToInt32(item.Value) - 1;
        }

        public static List<int> GetLineNos(this ICollection<MapParametersDto> parameters)
        {
            var item = parameters.FirstOrDefault(p => p.Key == ParameterKeys.LineNos.ToString());
            var list = item.Value.Split(",");
            return list.Select(int.Parse).ToList();
        }

        public static List<int> GetPageNos(this ICollection<MapParametersDto> parameters)
        {
            var item = parameters.FirstOrDefault(p => p.Key == ParameterKeys.PageNos.ToString());
            var list = item.Value.Split(",");
            return list.Select(int.Parse).ToList();
        }

        public static int GetStartIndex(this ICollection<MapParametersDto> parameters)
        {
            var item = parameters.FirstOrDefault(p => p.Key == ParameterKeys.StartIndex.ToString());
            return Convert.ToInt32(item.Value) - 1;
        }

        public static int GetEndIndex(this ICollection<MapParametersDto> parameters)
        {
            var item = parameters.FirstOrDefault(p => p.Key == ParameterKeys.EndIndex.ToString());
            return Convert.ToInt32(item.Value) - 1;
        }

        public static bool GetCaseSensitive(this ICollection<MapParametersDto> parameters,
            bool defaultValue = false)
        {
            return parameters.Any(p => p.Key == ParameterKeys.CaseSensitive.ToString());
        }

        public static bool GetMatchWholeWord(this ICollection<MapParametersDto> parameters)
        {
            return parameters.Any(p => p.Key == ParameterKeys.MatchWholeWord.ToString());
        }

        public static List<int> GetConcatFields(this ICollection<PostProcessMapParametersDto> parameters)
        {
            var item = parameters.FirstOrDefault(p => p.Key == ParameterKeys.ConcatFields.ToString());
            var list = item.Value.Split(",");
            return list.Select(int.Parse).ToList();

        }

        public static List<Tuple<string, string>> GetReplaceValues(this ICollection<PostProcessMapParametersDto> parameters)
        {
            var result = new List<Tuple<string, string>>();
            var item = parameters.FirstOrDefault(p => p.Key == ParameterKeys.ReplaceValues.ToString());
            var list = item.Value.Split(";", StringSplitOptions.RemoveEmptyEntries);
            if (list == null || list.Length == 0)
            {
                return result;
            }

            foreach (var listItem in list.ToList())
            {
                var items = listItem.Split("=>", StringSplitOptions.RemoveEmptyEntries);
                if (items == null || items.Length == 0)
                {
                    continue;
                }

                var key = items[0];
                var val = items.Length > 1 ? items[1] : string.Empty;

                result.Add(new Tuple<string, string>(key, val));
            }
            return result;
        }
    }
}
