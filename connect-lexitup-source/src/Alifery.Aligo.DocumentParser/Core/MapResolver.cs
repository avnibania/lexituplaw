using Alifery.Aligo.DocumentParser.Models;
using Alifery.Connect.CoreEntities.AligoEntities.DocumentParser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Alifery.Aligo.DocumentParser.Core
{
    public static class MapResolver
    {
        internal static string ProcessMap(DocumentContentModel model, ICollection<DocumentFieldMappingDto> mapping, List<KeyValuePair<string, string>> variables = null
        )
        {
            var value = string.Empty;
            foreach (var map in mapping.OrderBy(p => p.ExecutionOrder))
            {
                try
                {
                    value = ProcessMappingMethod(model, map.Map, variables);

                    if (!string.IsNullOrEmpty(value))
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    continue;
                }

            }
            return value;
        }

        internal static bool CheckFormatMatch(ICollection<DocumentFormatIdentifierDto> formatFormatIdentifiers,
           DocumentContentModel model)
        {
            if (null == formatFormatIdentifiers || formatFormatIdentifiers.Count == 0)
            {
                return false;
            }

            var groups = formatFormatIdentifiers.GroupBy(x => x.GroupKey);

            foreach (var group in groups)
            {
                var identified = true;
                foreach (var formatIdentifier in group.OrderBy(p => p.ExecutionSequence))
                {
                    foreach (var formatIdentifierMap in formatIdentifier.Mappings)
                    {
                        switch (formatIdentifier.IdentifierType)
                        {
                            case FormatIdentifierType.DocumentContentLocation:
                                try
                                {
                                    var map = formatIdentifierMap;

                                    var key = ProcessMappingMethod(model, map);
                                    var propertyValue = map.Parameters.GetStringParameterValue(ParameterKeys.IdentifierValue);
                                    var matchWholeWord = map.Parameters.GetMatchWholeWord();
                                    var match = matchWholeWord
                                        ? key == propertyValue
                                        : key.Contains(propertyValue);
                                    if (!match)
                                    {
                                        if (formatIdentifier.GroupMethod == FormatIdentifierGroupMethod.None || formatIdentifier.GroupMethod == FormatIdentifierGroupMethod.AllTrue)
                                        {
                                            identified = false;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    identified = false;
                                }
                                break;
                            case FormatIdentifierType.DocumentInfoField:
                                try
                                {
                                    var map = formatIdentifierMap;
                                    var propertyName = map.Parameters.GetStringParameterValue(ParameterKeys.PropertyName);
                                    var propertyValue = map.Parameters.GetStringParameterValue(ParameterKeys.IdentifierValue);
                                    var matchWholeWord = map.Parameters.GetMatchWholeWord();
                                    var key = model.DocumentInfo.GetType().GetProperty(propertyName)
                                        .GetValue(model.DocumentInfo, null);
                                    var match = matchWholeWord
                                        ? key.ToString() == propertyValue
                                            : key.ToString().Contains(propertyValue);
                                    if (!match)
                                    {
                                        if (formatIdentifier.GroupMethod == FormatIdentifierGroupMethod.None || formatIdentifier.GroupMethod == FormatIdentifierGroupMethod.AllTrue)
                                        {
                                            identified = false;
                                        }
                                    }

                                }
                                catch (Exception)
                                {
                                    identified = false;
                                }

                                break;
                        }
                    }
                }

                if (identified)
                {
                    return true;
                }
            }
            return false;
        }

        private static string ProcessMappingMethod(DocumentContentModel model, MapDto map, List<KeyValuePair<string, string>> variables = null)
        {
            var value = string.Empty;
            List<string> lines;
            string line;
            switch (map.SearchScope)
            {
                case SearchScope.Line:
                    var pageNo = map.Parameters.GetPageNo();
                    var lineNo = map.Parameters.GetLineNo();
                    var page = model.Pages[pageNo];
                    var firstLine = page.ContentLines[lineNo];
                    lines = new List<string> { firstLine };
                    line = map.LineIdentifierMethod.ExtractLine(lines, map.Parameters, variables);
                    value = map.FieldIdentifierMethod.ExtractValue(line, map.Parameters, variables);
                    break;
                case SearchScope.Document:
                    lines = new List<string>();
                    foreach (var thisPage in model.Pages)
                    {
                        lines.AddRange(thisPage.ContentLines);
                    }

                    line = map.LineIdentifierMethod.ExtractLine(lines, map.Parameters, variables);
                    value = map.FieldIdentifierMethod.ExtractValue(line, map.Parameters, variables);

                    break;

                case SearchScope.Page:
                    pageNo = map.Parameters.GetPageNo();
                    page = model.Pages[pageNo];
                    lines = page.ContentLines;
                    line = map.LineIdentifierMethod.ExtractLine(lines, map.Parameters, variables);
                    value = map.FieldIdentifierMethod.ExtractValue(line, map.Parameters, variables);
                    break;
                case SearchScope.StaticValue:
                    value = map.Parameters.GetStringParameterValue(ParameterKeys.StaticValue);
                    break;
            }

            var processedVal = value;
            foreach (var mapPostProcessMap in map.PostProcessMaps.OrderBy(p => p.ExecutionSequence))
            {
                processedVal = ProcessValue(model, processedVal, mapPostProcessMap);
            }

            return processedVal;
        }

        private static string ProcessValue(DocumentContentModel model, string value, PostProcessMapDto map)
        {
            var result = value;

            if (null == map)
            {
                return value;
            }

            switch (map.PostProcessAction)
            {
                case PostProcessAction.ConcatFieldValues:
                    var fields = map.Parameters.GetConcatFields();

                   
                    break;
                case PostProcessAction.DeriveFromMap:
                    
                    break;
                case PostProcessAction.RemoveExtraSpaces:
                    return value.Trim();
                case PostProcessAction.None:
                    return value;
                case PostProcessAction.ReplaceSubstringWithStaticValue:
                    var replaceValues = map.Parameters.GetReplaceValues();
                    foreach (var replaceValue in replaceValues)
                    {
                        result = result.Replace(replaceValue.Item1, replaceValue.Item2);
                    }
                    break;
                case PostProcessAction.ValidateDataType:
                    var dataType = map.Parameters.GetStringParameterValue(ParameterKeys.DataType);

                    var type = Enum.Parse(typeof(ProcessFieldType), dataType);
                    var flag = false;
                    switch (type)
                    {

                        case ProcessFieldType.Date:
                            var dateFormat = map.Parameters.GetStringParameterValue(ParameterKeys.DateFormat);
                            CultureInfo enAU = new CultureInfo("en-AU");
                            if (string.IsNullOrEmpty(dateFormat))
                            {
                                flag = DateTime.TryParse(result, out var dateValue);
                            }
                            else
                            {
                                flag = DateTime.TryParseExact(result.Trim(), dateFormat, enAU, DateTimeStyles.None, out var dateValue);
                            }

                            if (flag)
                            {
                                return result;
                            }
                            return string.Empty;
                        case ProcessFieldType.DateTime:
                            flag = DateTime.TryParse(result, out var dateTimeValue);
                            if (flag)
                            {
                                return result;
                            }
                            return string.Empty;

                        case ProcessFieldType.Number:
                            flag = Int64.TryParse(result, out var numberValue);
                            if (flag)
                            {
                                return result;
                            }
                            return string.Empty;
                    }
                    break;
            }
            return result;
        }



        public static string ExtractLine(this LineIdentifierMethod method, List<string> lines,
            ICollection<MapParametersDto> parameters, List<KeyValuePair<string, string>> variables = null)
        {
            if (null == lines)
            {
                return string.Empty;
            }

            var line = string.Empty;
            string lineIdentifierText;

            switch (method)
            {
                case LineIdentifierMethod.MatchAdjacentLineText:
                    lineIdentifierText = parameters.GetStringParameterValue(ParameterKeys.LineIdentifierText);
                    var noOfLines = parameters.GetNumericParameterValue(ParameterKeys.NoOfLines);

                    for (var i = 0; i < lines.Count; i++)
                    {
                        if (lines[i].Contains(lineIdentifierText))
                        {
                            if (i + noOfLines <= lines.Count)
                            {
                                return lines[i + noOfLines];
                            }
                        }
                    }

                    return string.Empty;
              
                case LineIdentifierMethod.MatchAdjacentLineTextAndJoinLines:
                    lineIdentifierText = parameters.GetStringParameterValue(ParameterKeys.LineIdentifierText);
                    var linesFrom = parameters.GetNumericParameterValue(ParameterKeys.JoinLinesFrom);
                    var linesTo = parameters.GetNumericParameterValue(ParameterKeys.JoinLinesTo);
                    var returnVal = string.Empty;
                    var identifiedLineIndex = 0;
                    for (var i = 0; i < lines.Count; i++)
                    {
                        if (lines[i].Contains(lineIdentifierText))
                        {
                            identifiedLineIndex = i;
                            break;
                        }
                    }
                    if (identifiedLineIndex + linesFrom <= lines.Count && identifiedLineIndex + linesTo <= lines.Count && identifiedLineIndex + linesFrom > 0 && identifiedLineIndex + linesTo > 0)
                    {
                        for (int i = identifiedLineIndex + linesFrom; i <= identifiedLineIndex + linesTo; i++)
                        {
                            returnVal += $" {lines[i]}";
                        }
                        return returnVal;
                    }
                    return returnVal;
                case LineIdentifierMethod.MatchText:
                    lineIdentifierText = parameters.GetStringParameterValue(ParameterKeys.LineIdentifierText);
                    return lines.Find(p => p.Contains(lineIdentifierText));

                case LineIdentifierMethod.MatchVariableValue:
                    if (null == variables)
                    {
                        return string.Empty;
                    }

                    var variableName = parameters.GetStringParameterValue(ParameterKeys.VariableName);
                    lineIdentifierText = variables.GetVariableValue(variableName);
                    return lines.Find(p => p.Contains(lineIdentifierText));
                case LineIdentifierMethod.MatchLength:
                    var length = parameters.GetNumericParameterValue(ParameterKeys.LineLength);
                    return lines.Find(p => p.Length == length);
                case LineIdentifierMethod.None:
                    return lines.FirstOrDefault();
            }
            return line;
        }

        public static string GetVariableValue(this List<KeyValuePair<string, string>> variables, string name)
        {
            return variables.FirstOrDefault(p => p.Key == name).Value;
        }

        public static string ExtractValue(this FieldIdentifierMethod method, string line,
            ICollection<MapParametersDto> parameters, List<KeyValuePair<string, string>> variables = null)
        {
            if (string.IsNullOrEmpty(line))
            {
                return string.Empty;
            }

            var value = string.Empty;
            string endIndexText;
            string startIndexText;
            bool caseSensitive;
            int length;
            int startIndex;
            int endIndex;
            switch (method)
            {
                case FieldIdentifierMethod.EndIndexAndLength:
                    endIndex = parameters.GetEndIndex();
                    length = parameters.GetNumericParameterValue(ParameterKeys.Length);
                    startIndex = endIndex - length;
                    return line.Substring(startIndex, length);

                case FieldIdentifierMethod.EndIndexOfTextAndLength:
                    endIndexText = parameters.GetStringParameterValue(ParameterKeys.EndIndexSearchText);
                    caseSensitive = parameters.GetCaseSensitive();
                    length = parameters.GetNumericParameterValue(ParameterKeys.Length);
                    endIndex = line.IndexOf(endIndexText,
                        caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
                    startIndex = endIndex - length;
                    return line.Substring(startIndex, length);
                case FieldIdentifierMethod.StartIndexAndLength:
                    startIndex = parameters.GetStartIndex();
                    length = parameters.GetNumericParameterValue(ParameterKeys.Length);
                    return line.Substring(startIndex, length);
                case FieldIdentifierMethod.StartIndex:
                    startIndex = parameters.GetStartIndex();
                    return line.Substring(startIndex);
                case FieldIdentifierMethod.StartIndexOfTextAndLength:
                    startIndexText = parameters.GetStringParameterValue(ParameterKeys.StartIndexSearchText);
                    caseSensitive = parameters.GetCaseSensitive();
                    startIndex = line.IndexOf(startIndexText,
                        caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
                    if (startIndex == -1)
                    {
                        return string.Empty;
                    }

                    startIndex += startIndexText.Length;
                    length = parameters.GetNumericParameterValue(ParameterKeys.Length);
                    return line.Substring(startIndex, length);
                case FieldIdentifierMethod.StartIndexOfText:
                    startIndexText = parameters.GetStringParameterValue(ParameterKeys.StartIndexSearchText);
                    caseSensitive = parameters.GetCaseSensitive();
                    startIndex = line.IndexOf(startIndexText,
                        caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
                    if (startIndex == -1)
                    {
                        return string.Empty;
                    }

                    startIndex += startIndexText.Length;
                    return line.Substring(startIndex);
                case FieldIdentifierMethod.StartIndexOfTextAndEndIndexOfText:
                    startIndexText = parameters.GetStringParameterValue(ParameterKeys.StartIndexSearchText);
                    endIndexText = parameters.GetStringParameterValue(ParameterKeys.EndIndexSearchText);
                    caseSensitive = parameters.GetCaseSensitive();
                    startIndex = line.IndexOf(startIndexText,
                        caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
                    if (startIndex == -1)
                    {
                        return string.Empty;
                    }

                    startIndex += startIndexText.Length;
                    endIndex = line.Substring(startIndex).IndexOf(endIndexText,
                        caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
                    endIndex = endIndex + startIndex;
                    length = endIndex - startIndex;
                    return line.Substring(startIndex, length);
                case FieldIdentifierMethod.StartIndexAndEndIndexOfText:
                    startIndex = parameters.GetStartIndex();
                    endIndexText = parameters.GetStringParameterValue(ParameterKeys.EndIndexSearchText);
                    caseSensitive = parameters.GetCaseSensitive();
                    if (startIndex == -1)
                    {
                        return string.Empty;
                    }

                    endIndex = line.Substring(startIndex).IndexOf(endIndexText,
                        caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
                    endIndex = endIndex + startIndex;
                    length = endIndex - startIndex;
                    return line.Substring(startIndex, length);
            }
            return value;
        }

    }
}
