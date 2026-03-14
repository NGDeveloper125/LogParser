using LogParser;
using LogParser.SectionParsers;

namespace LogParserTests;

public class LogLineParserTests
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void BuildLogLine_ShouldBeUnsuccessful_WhenLineIsEmptyOrNull(string? logLine)
    {
        LogLineFormat logLineFormat = new LogLineFormat([], []);

        LogLineParserResult logLineResult = LogLineParser.BuildLogLine(logLine!, logLineFormat);

        Assert.NotNull(logLineResult);
        Assert.False(logLineResult.Success);
    }

    [Fact]
    public void BuildLogLine_ShouldBeUnsuccessful_WhenLogLineRulesIsNull()
    {
        LogLineParserResult logLineResult = LogLineParser.BuildLogLine("Some log line", null!);

        Assert.NotNull(logLineResult);
        Assert.False(logLineResult.Success);
    }

    [Theory]
    [InlineData(new string[] { "DateTime" }, "This is a log line message")]
    public void BuildLogLine_ShouldBeUnsuccessful_WhenLogLineFormatIsNotInTheExpextedFormat(string[] SectionParserTypes, string logLine)
    {
        List<SectionParser> sectionParsers = new List<SectionParser>();
        foreach (string sectionsParserType in SectionParserTypes)
        {
            SectionParser sectionParser = Enum.Parse<SectionParser>(sectionsParserType);
            sectionParsers.Add(sectionParser);
        }
        LogLineFormat logLineFormat = new LogLineFormat(sectionParsers, []);

        LogLineParserResult logLineResult = LogLineParser.BuildLogLine(logLine, logLineFormat);

        Assert.NotNull(logLineResult);
        Assert.False(logLineResult.Success);
    }

    [Theory]
    [InlineData(new string[] { "DateTime" }, "2024-01-01 12:00:00 INFO This is a log line message")]
    [InlineData(new string[] { "LogLevel" }, "[INFO] This is a log line message")]
    [InlineData(new string[] { "Component" }, "UserService - This is a log line message")]
    [InlineData(new string[] { "LogMessage" }, "This is a log line message")]
    public void BuildLogLine_ShouldBeSuccessful_WhenLogLineFormatIsInTheExpextedFormat(string[] SectionParserTypes, string logLine)
    {
        List<SectionParser> sectionParsers = new List<SectionParser>();
        foreach (string sectionsParserType in SectionParserTypes)
        {
            SectionParser sectionParser = Enum.Parse<SectionParser>(sectionsParserType);
            sectionParsers.Add(sectionParser);
        }

        LogLineFormat logLineFormat = new LogLineFormat(sectionParsers, []);
        LogLineParserResult logLineResult = LogLineParser.BuildLogLine(logLine, logLineFormat);
        Assert.NotNull(logLineResult);
        Assert.True(logLineResult.Success);
    }

    [Theory]
    [InlineData(new string[] { "DateTime", "LogLevel", "Component", "LogMessage" }, "2024-01-01 12:00:00 INFO UserService - This is a log line message", new string[] { "2024-01-01 12:00:00", "INFO", "UserService", "This is a log line message" })]
    [InlineData(new string[] { "LogLevel", "DateTime", "Component", "LogMessage" }, "INFO 2024-01-01 12:00:00 UserService - This is a log line message", new string[] { "INFO", "2024-01-01 12:00:00", "UserService", "This is a log line message" })]
    [InlineData(new string[] { "Component", "LogLevel", "DateTime", "LogMessage" }, "UserService - INFO 2024-01-01 12:00:00 This is a log line message", new string[] { "UserService", "INFO", "2024-01-01 12:00:00", "This is a log line message" })]
    public void BuildLogLine_ShouldParseSectionsCorrectly_WhenLogLineFormatIsInTheExpextedFormat(string[] SectionParserTypes, string logLine, string[] expectedSections)
    {
        List<SectionParser> sectionParsers = new List<SectionParser>();
        foreach (string sectionsParserType in SectionParserTypes)
        {
            SectionParser sectionParser = Enum.Parse<SectionParser>(sectionsParserType);
            sectionParsers.Add(sectionParser);
        }
        LogLineFormat logLineFormat = new LogLineFormat(sectionParsers, []);
        LogLineParserResult logLineResult = LogLineParser.BuildLogLine(logLine, logLineFormat);

        Assert.NotNull(logLineResult);
        Assert.True(logLineResult.Success);
        Assert.Equal(expectedSections.Length, logLineResult.LogLineSections.Length);

        for (int i = 0; i < expectedSections.Length; i++)
        {
            Assert.Equal(expectedSections[i], logLineResult.LogLineSections[i]);
        }
    }
}
