using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reusable.Converters.Tests
{
    [TestClass]
    public class BooleanConvertersTest
    {
        [TestMethod]
        public void Convert_String_Boolean()
        {
            var converter = TypeConverter.Empty.Add<StringToBooleanConverter>();
            var result = converter.Convert("true", typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Convert_Boolean_String()
        {
            var converter = TypeConverter.Empty.Add<BooleanToStringConverter>();
            var result = converter.Convert(true, typeof(string));
            Assert.IsTrue(((string)result) == bool.TrueString);
        }

        [TestMethod]
        public void Convert_Boolean_Boolean()
        {
            var converter = TypeConverter.Empty.Add<StringToBooleanConverter>();
            var result = converter.Convert(true, typeof(bool));
            Assert.IsTrue((bool)result);
        }
    }
}