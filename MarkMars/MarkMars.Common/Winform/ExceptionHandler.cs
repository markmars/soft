using System;
using System.IO;
using System.Xml;

namespace MarkMars.Common
{
    public static class ExceptionHandler
    {
        /// <summary>
        /// 处理异常。
        /// </summary>
        /// <param name="ex">Exception 对象。</param>
        public static void HandleException(Exception ex)
        {
            try
            {
                String logDirectory = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Logs";
                String logFile = logDirectory + "\\" + DateTime.Today.ToString("yyyyMM") + ".xml";

                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                if (!File.Exists(logFile))
                {
                    String newLine = Environment.NewLine;
                    System.Text.StringBuilder logContent = new System.Text.StringBuilder();

                    logContent.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + newLine);
                    logContent.Append("<exceptions>" + newLine);
                    logContent.Append("</exceptions>");

                    File.WriteAllText(logFile, logContent.ToString(), System.Text.Encoding.UTF8);
                }

                XmlDocument logDoc = new XmlDocument();
                logDoc.Load(logFile);

                XmlElement itemElement = logDoc.CreateElement("exception");
                logDoc.DocumentElement.AppendChild(itemElement);

                XmlElement dateElement = logDoc.CreateElement("time");
                dateElement.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                itemElement.AppendChild(dateElement);

                XmlElement messageElement = logDoc.CreateElement("message");
                messageElement.InnerText = ex.Message;
                itemElement.AppendChild(messageElement);

                XmlElement stackTraceElement = logDoc.CreateElement("stackTrace");
                stackTraceElement.InnerText = ex.StackTrace;
                itemElement.AppendChild(stackTraceElement);

                logDoc.Save(logFile);
            }
            catch
            {
            }
            
            throw new Exception(ex.Message);
        }
    }
}
