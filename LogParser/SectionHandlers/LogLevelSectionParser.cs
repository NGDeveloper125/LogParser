namespace LogParser.SectionHandlers;

internal class LogLevelSectionParser : ISectionParser
{
    private static readonly string[] defaultLevels = ["TRACE", "DEBUG", "INFO", "WARN", "WARNING", "ERROR", "FATAL", "CRITICAL"];

    public LogLineParserResult ParseSection(string logLine, int sectionIndex)
    {
        if (string.IsNullOrEmpty(logLine))
            return new LogLineParserResult(false, [], "Failed to parse loglevel section in log line");

        var parts = SplitLogLine(logLine);

        if (sectionIndex >= 0 && sectionIndex < parts.Count)
        {
            var candidate = parts[sectionIndex].Trim().ToUpperInvariant();

            // Remove common brackets or prefixes
            candidate = candidate.Trim('[', ']', '(', ')');

            if (defaultLevels.Contains(candidate))
                return new LogLineParserResult(true, [candidate], string.Empty);
        }

        return new LogLineParserResult(false, [], "Failed to parse loglevel section in log line");
    }

    private List<string> SplitLogLine(string logLine)
    {
        var parts = new List<string>();
        var current = string.Empty;
        var inBracket = false;

        foreach (char c in logLine)
        {
            if (c == '[' || c == '(')
            {
                if (!string.IsNullOrWhiteSpace(current))
                    parts.Add(current.Trim());
                current = string.Empty;
                inBracket = true;
            }
            else if (c == ']' || c == ')')
            {
                if (!string.IsNullOrWhiteSpace(current))
                    parts.Add(current.Trim());
                current = string.Empty;
                inBracket = false;
            }
            else if ((c == ' ' || c == '\t' || c == '|') && !inBracket)
            {
                if (!string.IsNullOrWhiteSpace(current))
                    parts.Add(current.Trim());
                current = string.Empty;
            }
            else
            {
                current += c;
            }
        }

        if (!string.IsNullOrWhiteSpace(current))
            parts.Add(current.Trim());

        return parts;
    }
}
