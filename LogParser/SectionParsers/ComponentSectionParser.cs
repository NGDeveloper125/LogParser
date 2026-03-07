using System.Text.RegularExpressions;

namespace LogParser.SectionParsers;

internal class ComponentSectionParser : ISectionParser
{
    private static readonly string ComponentPattern = @"^[\s\-\|]*([A-Z][a-zA-Z]+)[\s\-\|]";

    public LogLineParserResult ParseSection(string logLine, int sectionIndex, List<string> parsedSections)
    {
        if (string.IsNullOrEmpty(logLine))
            return new LogLineParserResult(false, [], "Failed to parse component section in log line");

        var workingLine = RemoveParsedSections(logLine, parsedSections);

        var match = Regex.Match(workingLine, ComponentPattern);
        if (match.Success && match.Groups.Count > 1)
        {
            var componentName = match.Groups[1].Value;
            if (IsValidComponentName(componentName))
                return new LogLineParserResult(true, [componentName], string.Empty);
        }

        return new LogLineParserResult(false, [], "Failed to parse component section in log line");
    }

    private bool IsValidComponentName(string componentName)
    {
        return !string.IsNullOrEmpty(componentName) && 
               char.IsUpper(componentName[0]) && 
               componentName.Length > 1 &&
               componentName.All(char.IsLetter);
    }

    private string RemoveParsedSections(string logLine, List<string> parsedSections)
    {
        var remaining = logLine;
        foreach (var parsed in parsedSections)
        {
            if (!string.IsNullOrEmpty(parsed))
            {
                var index = remaining.IndexOf(parsed);
                if (index >= 0)
                {
                    remaining = remaining.Substring(index + parsed.Length);
                }
            }
        }
        return remaining;
    }
}