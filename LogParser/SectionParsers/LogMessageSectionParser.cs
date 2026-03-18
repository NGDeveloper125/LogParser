using System.Text.RegularExpressions;

namespace LogParser.SectionParsers;

internal class LogMessageSectionParser : ISectionParser
{
    public LogLineParserResult ParseSection(string logLine, int sectionIndex, List<string> parsedSections)
    {
        if (string.IsNullOrEmpty(logLine))
            return new LogLineParserResult(false, [], "Failed to parse message section in log line");

        // Check if any parsed section appears multiple times in the log line
        // If so, return the original log line to preserve context
        foreach (var parsed in parsedSections)
        {
            if (!string.IsNullOrEmpty(parsed))
            {
                int firstIndex = logLine.IndexOf(parsed);
                int lastIndex = logLine.LastIndexOf(parsed);
                if (firstIndex != lastIndex)
                {
                    // Section appears multiple times, return original log line
                    return new LogLineParserResult(true, [logLine], string.Empty);
                }
            }
        }

        // Bracket detection removed - relying on captureRatio check instead
        // The bracket check was too conservative and interfered with valid parsing
        // where brackets are consumed by other parsers (e.g., LogLevelSectionParser)

        var (message, consumedLength) = RemoveParsedSectionsWithTracking(logLine, parsedSections);

        message = message.TrimStart(' ', '-', '|', ':', '\t');

        // Check if the extracted message is significantly shorter than the original, indicating lost context
        // If the message plus parsed sections don't account for most of the log line, return the original
        // Include consumedLength to account for brackets and other content consumed but not returned by parsers
        int parsedLength = parsedSections.Where(s => !string.IsNullOrEmpty(s)).Sum(s => s.Length);
        int messageLength = !string.IsNullOrEmpty(message) ? message.Length : 0;
        double captureRatio = (double)(parsedLength + messageLength + consumedLength) / logLine.Length;

        if (captureRatio < 0.90)
        {
            // Significant content was lost during parsing, return original log line
            return new LogLineParserResult(true, [logLine], string.Empty);
        }

        if (!string.IsNullOrEmpty(message))
            return new LogLineParserResult(true, [message], string.Empty);

        return new LogLineParserResult(false, [], "Failed to parse message section in log line");
    }

    private (string, int) RemoveParsedSectionsWithTracking(string logLine, List<string> parsedSections)
    {
        var remaining = logLine;
        int totalConsumed = 0;

        foreach (var parsed in parsedSections)
        {
            if (!string.IsNullOrEmpty(parsed))
            {
                var index = remaining.IndexOf(parsed);
                if (index >= 0)
                {
                    // Check if there's unconsumed bracket content before this section
                    // This handles cases where LogLevelSectionParser matched brackets but didn't return them
                    var beforeParsed = remaining.Substring(0, index);
                    var bracketMatch = Regex.Match(beforeParsed.TrimStart(), @"^(\[[^\]]*\]\s*)");
                    if (bracketMatch.Success)
                    {
                        totalConsumed += bracketMatch.Length;
                    }

                    remaining = remaining.Substring(index + parsed.Length);
                }
            }
        }

        return (remaining, totalConsumed);
    }
}