﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using JetBrains.Annotations;
using Reusable.Collections;
using Reusable.Diagnostics;
using Reusable.Extensions;
using Reusable.OmniLog.Collections;

namespace Reusable.OmniLog
{
    /// <inheritdoc />
    /// <summary>
    /// The base interface for all loggers. The disposable interface can be used to unsubscribe any observers from the logger. It doesn't use any resources so dispose is optional.
    /// </summary>
    public interface ILogger : IDisposable
    {
        [AutoEqualityProperty]
        SoftString Name { get; }

        [NotNull]
        ILogger Log([NotNull] LogLevel logLevel, [NotNull] Action<Log> logAction);
    }

    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Logger : ILogger, IObservable<Log>, IEquatable<ILogger>
    {
        public static readonly IEnumerable<LogLevel> LogLevels;

        private bool _disposed;
        private readonly LogPredicate _logMatches;
        private readonly Action _dispose;
        private readonly ISet<IObserver<Log>> _observers;
        private readonly SoftString _name;
        private readonly LoggerPredicate _loggerMatches;

        static Logger()
        {
            LogLevels = new[]
            {
                LogLevel.Trace,
                LogLevel.Debug,
                LogLevel.Information,
                LogLevel.Warning,
                LogLevel.Error,
                LogLevel.Fatal,
            };
        }

        internal Logger([NotNull] SoftString name, [NotNull] LoggerPredicate loggerMatches, LogPredicate logMatches, Action dispose)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _loggerMatches = loggerMatches ?? throw new ArgumentNullException(nameof(loggerMatches));
            _logMatches = logMatches;
            _dispose = dispose;

            _observers = new HashSet<IObserver<Log>>();
        }

        private string DebuggerDisplay => DebuggerString.Create<Logger>(new { Name = _name, AttachementCount = Attachements.Count });

        internal HashSet<ILogAttachement> Attachements { get; set; } = new HashSet<ILogAttachement>();

        #region ILogger

        public SoftString Name => _name;

        public ILogger Log(LogLevel logLevel, Action<Log> logAction)
        {
            if (logLevel == null) throw new ArgumentNullException(nameof(logLevel));
            if (logAction == null) throw new ArgumentNullException(nameof(logAction));

            // Not sure whether this exception should be thrown.
            //if (_disposed) throw new InvalidOperationException($"This logger {Name.ToString().EncloseWith("()")} has been unsubscribed and cannot be used anymore.");

            if (_loggerMatches(logLevel, _name))
            {
                var log = new Log();
                log.Name(_name);
                log.LogLevel(logLevel);
                logAction(log);

                var rendered = log.Render(Attachements);

                if (_logMatches(log))
                {
                    foreach (var observer in _observers)
                    {
                        observer.OnNext(rendered);
                    }
                }
            }

            return this;
        }

        #endregion

        #region IEquatable

        public bool Equals(ILogger other) => AutoEquality<ILogger>.Comparer.Equals(this, other);

        public override bool Equals(object obj) => Equals(obj as ILogger);

        public override int GetHashCode() => AutoEquality<ILogger>.Comparer.GetHashCode(this);

        #endregion

        #region IObservable<Log>

        public IDisposable Subscribe(IObserver<Log> observer)
        {
            return
                _observers.Add(observer)
                    ? Disposable.Create(() => _observers.Remove(observer))
                    : Disposable.Empty;
        }

        #endregion

        public void Dispose()
        {
            _observers.Clear();
            _dispose();
            _disposed = true;
        }
    }
}