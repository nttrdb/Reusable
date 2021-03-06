using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using Reusable.Extensions;

namespace Reusable.Tests
{
    [TestClass]
    public class StringInterpolationTest
    {
       

        [TestMethod]
        public void Format_ReplacesName()
        {
            Assert.AreEqual(
                "The quick brown fox jumps over the lazy dog.",
                "The quick {Color} fox {Verb} over the lazy dog.".Format(new { Color = "brown", Verb = "jumps" }),
                "Couldn't replace all letter names.");

            Assert.AreEqual(
                "The quick brown fox jumps over the lazy dog.",
                "The quick {C} fox {V} over the lazy dog.".Format(new { C = "brown", V = "jumps" }),
                "Couldn't replace single letter names");

            Assert.AreEqual(
                "The quick brown fox jumps over the lazy dog.",
                "The quick {C1b} fox {V_9o} over the lazy dog.".Format(new { C1b = "brown", V_9o = "jumps" }),
                "Couldn't replace names with underscore and digits.");

            Assert.AreEqual(
                "The quick brown fox jumps over the lazy dog.",
                "The quick {_C1b} fox {V_9o} over the lazy dog.".Format(new { _C1b = "brown", V_9o = "jumps" }),
                "Couldn't replace names beginning with an underscore.");
        }

        [TestMethod]
        public void Format_NameWithAlignmentNotFound_UnchangedNameWithAlignment()
        {
            Assert.AreEqual(
                "The quick {brown,4} fox jumps over the lazy dog.",
                "The quick {brown,4} fox jumps over the lazy dog.".Format(new { fox = "dummy" }));
        }

        [TestMethod]
        public void Format_NameWithAlignmentAndFormatNotFound_UnchangedNameWithAlignmentAndFormat()
        {
            Assert.AreEqual(
                "The quick {brown,4:abc} fox jumps over the lazy dog.",
                "The quick {brown,4:abc} fox jumps over the lazy dog.".Format(new { fox = "dummy" }));
        }

        [TestMethod]
        public void Format_EscapedName_UnescapesName()
        {
            Assert.AreEqual(
                "The quick {brown} fox jumps over the lazy dog.",
                "The quick {{brown}} fox jumps over the lazy dog.".Format(new { fox = "dummy" }));
        }

        [TestMethod]
        public void Format_EscapedNameWithAlignment_UnescapesNameWithAlignment()
        {
            Assert.AreEqual(
                "The quick {brown,4} fox jumps over the lazy dog.",
                "The quick {{brown,4}} fox jumps over the lazy dog.".Format(new { fox = "dummy" }));
        }

        [TestMethod]
        public void Format_IgnoresInvalidBracePair()
        {
            Assert.AreEqual(
                "The quick {brown}} fox jumps over {{the} lazy dog.",
                "The quick {brown}} fox jumps over {{the} lazy dog.".Format(new { }));
        }

        [TestMethod]
        public void Format_NullValue_EmptyString()
        {
            Assert.AreEqual(
                "The quick {brown}} fox  over {{the} lazy dog.",
                "The quick {brown}} fox {Verb} over {{the} lazy dog.".Format(new { Verb = (string)null }));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Format_IgnoresNullObject()
        {
            Assert.AreEqual(
                "The quick {brown}} fox {Verb} over {{the} lazy dog.",
                "The quick {brown}} fox {Verb} over {{the} lazy dog.".Format((object)null));
        }

        [TestMethod]
        public void Format_IgnoresNullString()
        {
            Assert.IsNull(((string)null).Format((object)null));
        }

        [TestMethod]
        public void Format_NameWithDot_FormattedString()
        {
            Assert.AreEqual("foo waldo qux", "foo {bar.baz} qux".Format(new Dictionary<string, object> { ["bar.baz"] = "waldo" }));
        }

        [TestMethod]
        public void GetNames_NoNames_Empty()
        {
            var result = StringInterpolation.GetNames("foo bar baz").ToList();
            CollectionAssert.AreEqual(new string[] { }, result);
        }

        [TestMethod]
        public void GetNames_TextWithNames_Names()
        {
            var result = StringInterpolation.GetNames("foo {bar} baz {qux} waldo").ToList();
            CollectionAssert.AreEqual(new[] { "bar", "qux" }, result);
        }

        [TestMethod]
        public void FormatAll_Templates_Formats()
        {
            var constants = new Dictionary<string, object>
            {
                { "x", "foo {y} baz" },
                { "y", "bar {z} qux" },
                { "z", "waldo" },
            };

            Assert.AreEqual("foo bar waldo qux baz", "{x}".FormatAll(constants));
        }

    }

    [TestClass]
    public class ToJson_Exception
    {
        [TestMethod]
        public void CreateJsonString()
        {
            //try
            //{
            //    throw new FileNotFoundException("Bang!", @"C:\foo.txt");
            //}
            //catch (Exception ex)
            //{
            //    //var json = ex.ToJson();
            //}
        }
    }
}