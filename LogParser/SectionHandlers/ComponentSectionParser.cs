using System.Text.RegularExpressions;

namespace LogParser.SectionHandlers;

internal class ComponentSectionParser : ISectionParser
{
    private static readonly string componentPattern = @"^[A-Z][a-zA-Z]*";

    public LogLineParserResult ParseSection(string logLine, int sectionIndex)
    {
        if (string.IsNullOrEmpty(logLine))
            return new LogLineParserResult(false, [], "Failed to parse component section in log line");

        var parts = SplitLogLine(logLine);

        if (sectionIndex >= 0 && sectionIndex < parts.Count)
        {
            var candidate = parts[sectionIndex].Trim();
            var match = Regex.Match(candidate, componentPattern);
            if (match.Success && IsValidComponentName(match.Value))
                return new LogLineParserResult(true, [match.Value], string.Empty);
        }

        return new LogLineParserResult(false, [], "Failed to parse component section in log line");
    }

    private bool IsValidComponentName(string componentName)
    {
        return !string.IsNullOrEmpty(componentName) && 
               char.IsUpper(componentName[0]) && 
               componentName.All(char.IsLetter);
    }

    private List<string> SplitLogLine(string logLine)
    {
        var pattern = @"(\[[^\]]+\]|[^\s\[\|]+|\|)";
        var matches = Regex.Matches(logLine, pattern);

        var parts = new List<string>();
        foreach (Match match in matches)
        {
            var value = match.Value.Trim();
            if (!string.IsNullOrEmpty(value) && value != "|")
                parts.Add(value);
        }

        return parts;
    }
}