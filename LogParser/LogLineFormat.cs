using LogParser.SectionHandlers;

namespace LogParser;

public class LogLineFormat
{
    private List<ISectionHandler> SectionHandlers = [];

    public LogLineFormat(List<SectionHandler> sectionHandlers)
    {
        foreach (SectionHandler sectionHandler in sectionHandlers)
        {
            ISectionHandler handler = sectionHandler switch
            {
                SectionHandler.DateTime => new DateTimeSectionHandler(),
              //  SectionHandler.LogLevel => new LogLevelSectionHandler(),
              //  SectionHandler.LogMessage => new LogMessageSectionHandler(),
              //  SectionHandler.Component => new ComponentSectionHandler(),
                _ => throw new ArgumentException($"Invalid section handler type: {sectionHandler}")
            };
            SectionHandlers.Add(handler);
        }
    }

    public virtual int GetNumberOfSections()
    {
        return SectionHandlers.Count();
    }

    public virtual List<ISectionHandler> GetSectionHandlers()
    {
        return SectionHandlers;
    }
}
