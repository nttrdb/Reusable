﻿using System.Collections.Generic;
using System.Linq;
using Reusable.Collections;
using Reusable.OmniLog.Collections;

namespace Reusable.OmniLog
{
    public static class LogRenderer
    {
        internal static Log Render(this Log log, IEnumerable<ILogAttachement> attachements)
        {
            // Recreate the log with all properties and compute them.

            var properties =
                log.Select(l => (l.Key, l.Value))
                    .Concat(attachements.Select(a => (a.Name, (object)a)))
                    .Concat(LogScope.Current.Flatten().Select(scope => (Key: scope.Name, Value: (object) scope)));

            log = new Log().AddRange(properties);
            return log.Compute(log);
        }

        private static Log Compute(this Log source, Log rawLog)
        {
            var log = new Log();

            foreach (var item in source)
            {
                switch (item.Value)
                {
                    // Don't render LogLevel because we cannot filter without it.
                    //				case LogLevel logLevel:
                    //					log[item.Key] = logLevel.ToString();
                    //					break;s

                    case MessageFunc messageFunc:
                        log[LogProperty.Message] = messageFunc();
                        break;

                    case ILogAttachement computable:
                        log[item.Key] = computable.Compute(rawLog);
                        break;                    

                    case LogScope scope:
                        log[item.Key] = scope.Compute(rawLog);
                        break;

                    case LogBag bag:
                        // Skip the log-bag because it's not renderable.
                        continue;

                    default:
                        log[item.Key] = item.Value;
                        break;
                }
            }

            return log;
        }
    }
}