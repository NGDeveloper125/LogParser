using System;
namespace LogParser.SectionHandlers;

public interface ISectionParser
{
    LogLineParserResult ParseSection(string logLine, int sectionIndex);
}
