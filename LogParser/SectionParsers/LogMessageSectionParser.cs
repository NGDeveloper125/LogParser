namespace LogParser.SectionParsers;

internal class LogMessageSectionParser : ISectionParser
{
    public LogLineParserResult ParseSection(string logLine, int sectionIndex, List<string> parsedSections)
    {
        if (string.IsNullOrEmpty(logLine))
            return new LogLineParserResult(false, [], "Failed to parse message section in log line");

        var message = RemoveParsedSections(logLine, parsedSections);

        message = message.TrimStart(' ', '-', '|', ':', '\t');

        if (!string.IsNullOrEmpty(message))
            return new LogLineParserResult(true, [message], string.Empty);

        return new LogLineParserResult(false, [], "Failed to parse message section in log line");
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