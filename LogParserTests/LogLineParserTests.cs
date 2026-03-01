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
        List<SectionHandler> sectionHandlers = new List<SectionHandler>();
        foreach (string sectionsHandlerType in SectionHandlerTypes)
        { 
            SectionHandler sectionHandler = Enum.Parse<SectionHandler>(sectionsHandlerType);
            sectionHandlers.Add(sectionHandler);
        }
        LogLineFormat logLineFormat = new LogLineFormat(sectionHandlers);

        LogLineParserResult logLineResult = LogLineParser.BuildLogLine(logLine, logLineFormat);

        Assert.NotNull(logLineResult);
        Assert.False(logLineResult.Success);
    }
}
