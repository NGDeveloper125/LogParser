using System;
using System.Collections.Generic;
using System.Text;

namespace LogParser.SectionHandlers;

public interface ISectionHandler
{
    LogLineParserResult HandleSection(string logLine, int sectionIndex);
}
