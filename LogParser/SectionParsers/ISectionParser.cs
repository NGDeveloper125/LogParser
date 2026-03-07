using System;
namespace LogParser.SectionParsers;

public interface ISectionParser
{
    LogLineParserResult ParseSection(string logLine, int sectionIndex, List<string> parsedSections);
}
