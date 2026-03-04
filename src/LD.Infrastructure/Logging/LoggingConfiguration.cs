using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Infrastructure.Logging
{
    public static class LoggingConfiguration
    {
        public static LoggerConfiguration AddInfrastructureLogging(
            this LoggerConfiguration logger)
        {
            return logger

                .Enrich.FromLogContext();
        }
    }
}
