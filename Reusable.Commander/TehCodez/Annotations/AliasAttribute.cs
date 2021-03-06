﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Reusable.Extensions;

// ReSharper disable once CheckNamespace
namespace Reusable.CommandLine
{
    [UsedImplicitly]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
    public class AliasAttribute : Attribute, IEnumerable<SoftString>
    {
        private readonly IEnumerable<SoftString> _values;
        
        public AliasAttribute([NotNull] params string[] values)
        {
            _values = values.Select(SoftString.Create).ToList();
        }

        public IEnumerator<SoftString> GetEnumerator() => _values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}