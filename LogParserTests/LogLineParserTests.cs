using LogParser;
using LogParser.SectionHandlers;

namespace LogParserTests;

public class LogLineParserTests
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void BuildLogLine_ShouldBeUnsuccessful_WhenLineIsEmptyOrNull(string? logLine)
    {
        LogLineFormat logLineFormat = new LogLineFormat([]);

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
    public void BuildLogLine_ShouldBeUnsuccessful_WhenLogLineFormatIsNotInTheExpextedFormat(string[] SectionHandlerTypes, string logLine)
    {
        List<SectionParser> sectionHandlers = new List<SectionParser>();
        foreach (string sectionsHandlerType in SectionHandlerTypes)
        { 
            SectionParser sectionHandler = Enum.Parse<SectionParser>(sectionsHandlerType);
            sectionHandlers.Add(sectionHandler);
        }
        LogLineFormat logLineFormat = new LogLineFormat(sectionHandlers);

        LogLineParserResult logLineResult = LogLineParser.BuildLogLine(logLine, logLineFormat);

        Assert.NotNull(logLineResult);
        Assert.False(logLineResult.Success);
    }

    [Theory]
    [InlineData(new string[] { "DateTime" }, "2024-01-01 12:00:00 INFO This is a log line message")]
    [InlineData(new string[] { "LogLevel" }, "[INFO] This is a log line message")]
    [InlineData(new string[] { "Component" }, "UserService - This is a log line message")]
    public void BuildLogLine_ShouldBeSuccessful_WhenLogLineFormatIsInTheExpextedFormat(string[] SectionHandlerTypes, string logLine)
    {
        List<SectionParser> sectionHandlers = new List<SectionParser>();
        foreach (string sectionsHandlerType in SectionHandlerTypes)
        {
            SectionParser sectionHandler = Enum.Parse<SectionParser>(sectionsHandlerType);
            sectionHandlers.Add(sectionHandler);
        }

        LogLineFormat logLineFormat = new LogLineFormat(sectionHandlers);
        LogLineParserResult logLineResult = LogLineParser.BuildLogLine(logLine, logLineFormat);
        Assert.NotNull(logLineResult);
        Assert.True(logLineResult.Success);
    }
}
