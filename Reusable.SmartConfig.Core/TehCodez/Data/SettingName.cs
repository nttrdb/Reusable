﻿using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Reusable.Exceptionize;
using Reusable.Extensions;

namespace Reusable.SmartConfig.Data
{
    public class SettingName
    {
        public const string NamespaceSeparator = "+";
        public const string TypeSeparator = ".";
        public const string InstanceSeparator = ",";

        private static readonly string NamePattern =
            $"(?:(?<Namespace>[a-z0-9_.]+)\\{NamespaceSeparator})?" +
            $"(?:(?<Type>[a-z0-9_]+)\\{TypeSeparator})?" +
            $"(?<Property>[a-z0-9_]+)" +
            $"(?:{InstanceSeparator}(?<Instance>[a-z0-9_]+))?";

        [NotNull]
        private string _property;

        public SettingName([NotNull] string property)
        {
            _property = property ?? throw new ArgumentNullException(nameof(property));
        }

        public SettingName(SettingName settingName) : this(settingName.Property)
        {
            Namespace = settingName.Namespace;
            Type = settingName.Type;
            Instance = settingName.Instance;
        }

        [CanBeNull]
        public string Namespace { get; set; }

        [CanBeNull]
        public string Type { get; set; }

        [NotNull]
        public string Property
        {
            get => _property;
            set => _property = value ?? throw new ArgumentNullException(nameof(Property));
        }

        [CanBeNull]
        public string Instance { get; set; }

        [ContractAnnotation("value: null => halt"), NotNull]
        public static SettingName Parse([NotNull] string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var match = Regex.Match(value, NamePattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return new SettingName(match.Groups["Property"].Value)
                {
                    Namespace = match.Groups["Namespace"].Value.NullIfEmpty(),
                    Type = match.Groups["Type"].Value.NullIfEmpty(),
                    Instance = match.Groups["Instance"].Value.NullIfEmpty(),
                };
            }

            var settingNameFormat =
                $"[{nameof(Namespace)}{NamespaceSeparator}]" +
                $"[{nameof(Type)}{TypeSeparator}]" +
                $"{nameof(Property)}" +
                $"[{InstanceSeparator}{nameof(Instance)}]";

            throw ("SettingNameFormatException", $"Could not parse setting {value.QuoteWith("'")}. Expected format: {settingNameFormat}").ToDynamicException();
        }

        public override string ToString()
        {
            return new StringBuilder()
                .AppendWhen(!Namespace.IsNullOrEmpty(), () => $"{Namespace}{NamespaceSeparator}")
                .AppendWhen(!Type.IsNullOrEmpty(), () => $"{Type}{TypeSeparator}")
                .Append(Property)
                .AppendWhen(!Instance.IsNullOrEmpty(), () => $"{InstanceSeparator}{Instance}")
                .ToString();
        }

        public static implicit operator string(SettingName settingName) => settingName?.ToString();

        public static implicit operator SoftString(SettingName settingName) => settingName?.ToString();
    }
}