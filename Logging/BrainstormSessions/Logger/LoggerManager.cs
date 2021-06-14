using System;
using System.IO;
using System.Reflection;
using System.Xml;
using log4net;
using log4net.Config;

namespace BrainstormSessions.Logger
{
    public class LoggerManager
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(LoggerManager));

        public static ILog Log
        {
            get { return _logger; }
        }

        public static void InitLogger()
        {
            try
            {
                XmlDocument log4NetConfig = new XmlDocument();

                using (var fs = File.OpenRead("log4net.config"))
                {
                    log4NetConfig.Load(fs);

                    var repo = LogManager.CreateRepository(
                        Assembly.GetEntryAssembly(),
                        typeof(log4net.Repository.Hierarchy.Hierarchy));

                    XmlConfigurator.Configure(repo, log4NetConfig["log4net"]);

                    // The first log to be written 
                    _logger.Info("Log System Initialized");

                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error", ex);
            }
        }
    }
}
