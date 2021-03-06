﻿using System;
using System.Globalization;

namespace Reusable.Converters
{
    public class StringToInt32Converter : TypeConverter<String, Int32>
    {
        protected override int ConvertCore(IConversionContext<string> context)
        {
            return Int32.Parse(context.Value, NumberStyles.Integer, context.FormatProvider);
        }
    }

    public class Int32ToStringConverter : TypeConverter<Int32, String>
    {
        protected override string ConvertCore(IConversionContext<int> context)
        {
            return context.Value.ToString(context.FormatProvider);
        }
    }
}
