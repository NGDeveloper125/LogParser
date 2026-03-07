using System.Text.RegularExpressions;

namespace LogParser.SectionParsers;

internal class DateTimeSectionParser : ISectionParser
{
    private static readonly string defaultPattern =
    @"\d{4}-\d{2}-\d{2}[T\s]\d{2}:\d{2}:\d{2}(?:\.\d{3,6})?(?:Z|[+-]\d{2}:\d{2})?";

    public LogLineParserResult ParseSection(string logLine, int sectionIndex, List<string> parsedSections)
    {
        if (string.IsNullOrEmpty(logLine))
            return new LogLineParserResult(false, [], "Failed to parse datetime section in log line");

        var workingLine = RemoveParsedSections(logLine, parsedSections);

        var match = Regex.Match(workingLine.TrimStart(), defaultPattern);
        if (match.Success)
            return new LogLineParserResult(true, [match.Value], string.Empty);

        return new LogLineParserResult(false, [], "Failed to parse datetime section in log line");
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
