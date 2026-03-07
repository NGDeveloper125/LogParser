using System.Text.RegularExpressions;

namespace LogParser.SectionParsers;

internal class LogLevelSectionParser : ISectionParser
{
    private static readonly string[] defaultLevels = ["TRACE", "DEBUG", "INFO", "WARN", "WARNING", "ERROR", "FATAL", "CRITICAL"];

    public LogLineParserResult ParseSection(string logLine, int sectionIndex, List<string> parsedSections)
    {
        if (string.IsNullOrEmpty(logLine))
            return new LogLineParserResult(false, [], "Failed to parse loglevel section in log line");

        var workingLine = RemoveParsedSections(logLine, parsedSections);

        var pattern = @"^\s*[\-\|\s]*[\[\(]?(" + string.Join("|", defaultLevels) + @")[\]\)]?\s*";
        var match = Regex.Match(workingLine, pattern, RegexOptions.IgnoreCase);
        
        if (match.Success)
        {
            var level = match.Groups[1].Value.ToUpperInvariant();
            return new LogLineParserResult(true, [level], string.Empty);
        }

        return new LogLineParserResult(false, [], "Failed to parse loglevel section in log line");
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
