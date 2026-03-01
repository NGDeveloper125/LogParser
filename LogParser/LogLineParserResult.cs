namespace LogParser;

public record LogLineParserResult(bool Success, string[] LogLineSections, string ErrorMessage);
