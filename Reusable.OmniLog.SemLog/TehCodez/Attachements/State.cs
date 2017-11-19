﻿using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Reusable.OmniLog.Collections;

namespace Reusable.OmniLog.SemLog.Attachements
{
    public abstract class State : LogAttachement
    {
        protected State(string name) : base(name)
        {
            Settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                Converters = { new StringEnumConverter() }
            };
        }

        [NotNull]
        public JsonSerializerSettings Settings { get; set; }

        public override object Compute(Log log)
        {
            if (log.TryGetValue("Bag", out var bag) && bag is LogBag b && b.TryGetValue(Name.ToString(), out var obj))
            {
                return obj is string ? obj : JsonConvert.SerializeObject(obj, Settings);
            }
            return null;
        }
    }
}