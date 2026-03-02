using System.Text.RegularExpressions;

namespace LogParser.SectionHandlers;

internal class DateTimeSectionParser : ISectionParser
{
    private static readonly string defaultPattern =
    @"^\d{4}-\d{2}-\d{2}[T\s]\d{2}:\d{2}:\d{2}(?:\.\d{3,6})?(?:Z|[+-]\d{2}:\d{2})?";

    public LogLineParserResult ParseSection(string logLine, int sectionIndex)
    {
        if (string.IsNullOrEmpty(logLine))
            return new LogLineParserResult(false, [], "Failed to parse datetime section in log line");

        // Split by common delimiters but keep track of positions
        var parts = SplitLogLine(logLine);

        // If sectionIndex is within bounds, try to parse that specific section
        if (sectionIndex >= 0 && sectionIndex < parts.Count)
        {
            var candidate = parts[sectionIndex].Trim();
            var match = Regex.Match(candidate, defaultPattern);
            if (match.Success)
                return new LogLineParserResult(true, [match.Value], string.Empty);
        }

        // Fallback: if sectionIndex is 0, assume timestamp is at the beginning
        if (sectionIndex == 0)
        {
            var match = Regex.Match(logLine, defaultPattern);
            if (match.Success)
                return new LogLineParserResult(true, [match.Value], string.Empty);
        }

        return new LogLineParserResult(false, [], "Failed to parse datetime section in log line");
    }

    private List<string> SplitLogLine(string logLine)
    {
        // Split by brackets, pipes, tabs, or multiple spaces
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
