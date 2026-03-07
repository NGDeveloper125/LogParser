using LogParser.SectionParsers;

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
                return new LogLineParserResult(false, [], ex.Message);
            }
        }       
    
        private static LogLineParserResult BreakLogLineintoSections(string logLine, LogLineFormat logLineFormat)
        {
            List<string> logLineSections = new List<string>();
            int sectionIndex = 0;
            bool anySuccessful = false;
            
            foreach (ISectionParser sectionParser in logLineFormat.GetSectionParsers())
            {
                var result = sectionParser.ParseSection(logLine, sectionIndex, logLineSections);
                
                if (result.Success && result.LogLineSections.Length > 0)
                {
                    logLineSections.Add(result.LogLineSections[0]);
                    anySuccessful = true;
                }
                else
                {
                    logLineSections.Add(string.Empty);
                }
                
                sectionIndex++;
            }

            return new LogLineParserResult(anySuccessful, logLineSections.ToArray(), string.Empty);
        }
    }
}
