using System;
using Serilog;

namespace MainServer.Api.Common
{
    public static class Serilogger
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
           (context, configuration) =>
           {
               configuration
                    .WriteTo.Debug()
                    .WriteTo.Console()
                    .ReadFrom.Configuration(context.Configuration);
           };
    }
}

