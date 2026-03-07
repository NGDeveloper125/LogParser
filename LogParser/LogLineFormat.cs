using LogParser.SectionParsers;

namespace LogParser;

public class LogLineFormat
{
    private List<ISectionParser> SectionHandlers = [];

    public LogLineFormat(List<SectionParser> sectionHandlers)
    {
        foreach (SectionParser sectionHandler in sectionHandlers)
        {
            ISectionParser handler = sectionHandler switch
            {
                SectionParser.DateTime => new DateTimeSectionParser(),
                SectionParser.LogLevel => new LogLevelSectionParser(),
                SectionParser.Component => new ComponentSectionParser(),
                SectionParser.LogMessage => new LogMessageSectionParser(),
                _ => throw new ArgumentException($"Invalid section handler type: {sectionHandler}")
            };
            SectionHandlers.Add(handler);
        }
    }

    public virtual int GetNumberOfSections()
    {
        return SectionHandlers.Count();
    }

    public virtual List<ISectionParser> GetSectionParsers()
    {
        return SectionHandlers;
    }
}
