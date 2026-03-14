using LogParser.SectionParsers;

namespace LogParser;

public class LogLineFormat
{
    private List<ISectionParser> SectionHandlers = [];

    public LogLineFormat(List<SectionParser> sectionHandlers, Dictionary<int, ISectionParser> customSectionParsers)
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

        foreach (var kvp in customSectionParsers)
        {
            int index = kvp.Key;
            ISectionParser parser = kvp.Value;
            if (index < 0 || index > SectionHandlers.Count)
                throw new ArgumentException($"Custom section parser index {index} is out of range.");
            if (parser == null)
                throw new ArgumentNullException($"Custom section parser at index {index} cannot be null.");
            SectionHandlers.Insert(index, parser);
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
