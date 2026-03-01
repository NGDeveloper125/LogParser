using LogParser.SectionHandlers;

namespace LogParser
{
    public class LogLineParser
    {
        public static LogLineParserResult BuildLogLine(string logLine, LogLineFormat logLineFormat)
        {
            if (string.IsNullOrEmpty(logLine) || logLineFormat is null || logLineFormat.GetNumberOfSections() == 0) 
                return new LogLineParserResult(false, [], "LogLine nad LogLineFormat are null or empty");

            try
            {
                return BreakLogLineintoSections(logLine, logLineFormat);
            }
            catch (Exception ex)
            {
                return new LogLineParserResult(true, [], ex.Message);
            }
        }

        private static LogLineParserResult BreakLogLineintoSections(string logLine, LogLineFormat logLineFormat)
        {
            int numberOfSections = logLineFormat.GetNumberOfSections();
            List<string> logLineSections = new List<string>();
            int sectionIndex = 0;   
            foreach (ISectionHandler sectionHandler in logLineFormat.GetSectionHandlers())
            {
                LogLineParserResult sectionResult = sectionHandler.HandleSection(logLine, sectionIndex);
                if(!sectionResult.Success) return sectionResult;
                logLineSections.Add(sectionResult.LogLineSections[0]);
            }

            return new LogLineParserResult(true, logLineSections.ToArray(), string.Empty);
        }
    }
}
