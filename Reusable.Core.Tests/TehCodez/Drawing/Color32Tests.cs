﻿using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reusable.Drawing;
using Reusable.Formatters;

namespace Reusable.Tests.Drawing
{
    [TestClass]
    public class ctor_String
    {
        [TestMethod]
        public void CreateColor32FromName()
        {
            Assert.AreEqual(new Color32("red").ToArgb(), Color.FromName("red").ToArgb());
        }

        [TestMethod]
        public void CreatesColor32FromDec()
        {
            Assert.AreEqual(Color.FromArgb(155, 222, 33, 43).ToArgb(), new Color32(155, 222, 33, 43).ToArgb());
        }

        [TestMethod]
        public void CreatesColor32FromHex()
        {
            Assert.AreEqual(Color.FromArgb(155, 222, 33, 43).ToArgb(), new Color32("#9bde212b").ToArgb());
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ThrowsInvalidColorException()
        {
            new Color32("foo");
        }
    }

    [TestClass]
    public class _implicit_operator_Color32
    {
        [TestMethod]
        public void CastsToColor32()
        {
            
        }
    }

    [TestClass]
    public class _SerializationTests
    {
        [TestMethod]
        public void SerializesToHexWithoutAlpha()
        {
            var color32 = new Color32(Color.Chartreuse);
            var hex = color32.ToString("#{0:0xRGB}", new HexadecimalColorFormatter());
            Assert.AreEqual("#7FFF00", hex);
        }

        [TestMethod]
        public void SerializesToHexWithAlpha()
        {
            Assert.AreEqual("9BDE212B", new Color32("155, 222, 33, 43").ToString("{0:0xARGB}", new HexadecimalColorFormatter()));
        }
    }
}
