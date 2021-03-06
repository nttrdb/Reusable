﻿using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reusable;
using Reusable.Exceptionize;
using Reusable.Tester;

// ReSharper disable EqualExpressionComparison

namespace Reusable.Tests
{
    [TestClass]
    public class SemanticVersionTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ctor_MajorVersionLessThenZero_VersionOutOfRangeException()
        {
            new SemanticVersion { Major = -1, Minor = 1, Patch = 1 };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ctor_MinorVersionLessThenZero_VersionOutOfRangeException()
        {
            new SemanticVersion { Major = 1, Minor = -1, Patch = 1 };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ctor_PatchVersionLessThenZero_VersionOutOfRangeException()
        {
            new SemanticVersion { Major = 1, Minor = 1, Patch = -1 };
        }

        [TestMethod]
        public void Parse_ValidVersions_SemVer()
        {
            Assert.IsNotNull(SemanticVersion.Parse("0.0.0"));
            Assert.IsNotNull(SemanticVersion.Parse("1.0.0"));
            Assert.IsNotNull(SemanticVersion.Parse("0.1.0"));
            Assert.IsNotNull(SemanticVersion.Parse("0.0.1"));
            Assert.IsNotNull(SemanticVersion.Parse("1.0.0-label1"));
            Assert.IsNotNull(SemanticVersion.Parse("1.0.0-label1.label2"));
        }

        [TestMethod]
        public void Parse_InvalidMajorVersion_InvalidVersionException()
        {
            Assert.That.ThrowsExceptionFiltered<DynamicException>(() => SemanticVersion.Parse("01.0.0"), ex => ex.NameEquals("VersionFormatException"));
        }

        [TestMethod]
        public void Parse_InvalidMinorVersion_InvalidVersionException()
        {
            Assert.That.ThrowsExceptionFiltered<DynamicException>(() => SemanticVersion.Parse("0.01.0"), ex => ex.NameEquals("VersionFormatException"));
        }

        [TestMethod]
        public void Parse_InvalidPatchVersion_InvalidVersionException()
        {
            Assert.That.ThrowsExceptionFiltered<DynamicException>(() => SemanticVersion.Parse("0.0.01"), ex => ex.NameEquals("VersionFormatException"));
        }

#pragma warning disable 1718
        [TestMethod]
        public void Equals_VariousVersions()
        {
            Assert.IsTrue(SemanticVersion.Parse("1.0.0") == SemanticVersion.Parse("1.0.0"));
            Assert.IsFalse(SemanticVersion.Parse("1.1.0") == null);
            var semVer = SemanticVersion.Parse("1.1.1");
            Assert.IsTrue(semVer == semVer);
        }
#pragma warning restore 1718

        [TestMethod]
        public void operator_Equals_Null()
        {
            Assert.IsFalse(default(SemanticVersion) == default(SemanticVersion));
        }

        [TestMethod]
        public void CompareTo_LessThen()
        {
            Assert.IsTrue(SemanticVersion.Parse("1.0.0") < SemanticVersion.Parse("2.0.0"));
            Assert.IsTrue(SemanticVersion.Parse("1.1.0") < SemanticVersion.Parse("1.22.0"));
            Assert.IsTrue(SemanticVersion.Parse("1.1.1") < SemanticVersion.Parse("1.1.2"));
        }

        [TestMethod]
        public void CompareTo_Equal()
        {
            Assert.IsTrue(SemanticVersion.Parse("1.0.0") == SemanticVersion.Parse("1.0.0"));
            Assert.IsTrue(SemanticVersion.Parse("1.1.0") == SemanticVersion.Parse("1.1.0"));
            Assert.IsTrue(SemanticVersion.Parse("1.1.1") == SemanticVersion.Parse("1.1.1"));
        }

        [TestMethod]
        public void CompareTo_GreaterThen()
        {
            Assert.IsTrue(SemanticVersion.Parse("2.0.0") > SemanticVersion.Parse("1.0.0"));
            Assert.IsTrue(SemanticVersion.Parse("2.2.0") > SemanticVersion.Parse("2.1.0"));
            Assert.IsTrue(SemanticVersion.Parse("2.2.2") > SemanticVersion.Parse("2.2.1"));
        }

        [TestMethod]
        public void CompareTo_LessThenOrEqual()
        {
            Assert.IsTrue(SemanticVersion.Parse("1.0.0") <= SemanticVersion.Parse("2.0.0"));
            Assert.IsTrue(SemanticVersion.Parse("1.12.123") <= SemanticVersion.Parse("1.12.123"));
        }

        [TestMethod]
        public void CompareTo_GreaterThenOrEqual()
        {
            Assert.IsTrue(SemanticVersion.Parse("2.0.1") >= SemanticVersion.Parse("2.0.0"));
            Assert.IsTrue(SemanticVersion.Parse("2.23.234") >= SemanticVersion.Parse("2.23.234"));
        }

        [TestMethod]
        public void Sort_WithoutLabels()
        {
            // 1.0.0 < 2.0.0 < 2.1.0 < 2.1.1.

            var actual = new[]
            {
                "2.1.0",
                "2.0.0",
                "2.1.1",
                "1.0.0",
            }
            .Select(SemanticVersion.Parse)
            .OrderBy(x => x)
            .Select(x => x.ToString())
            .ToList();

            var expected = new[]
            {
                "1.0.0",
                "2.0.0",
                "2.1.0",
                "2.1.1",
            };

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Sort_WithLabels()
        {
            // Example: 1.0.0-alpha < 1.0.0-alpha.1 < 1.0.0-alpha.beta < 1.0.0-beta < 1.0.0-beta.2 < 1.0.0-beta.11 < 1.0.0-rc.1 < 1.0.0.

            var actual = new[]
            {
                "1.0.0-beta.11",
                "1.0.0-alpha.beta",
                "1.0.0-alpha.1",
                "1.0.0-rc.1",
                "1.0.0-alpha",
                "1.0.0-beta.2",
                "1.0.0-beta",
                "1.0.0",
            }
            .Select(SemanticVersion.Parse)
            .OrderBy(x => x)
            .Select(x => x.ToString())
            .ToList();

            var exptected = new[]
            {
                "1.0.0-alpha",
                "1.0.0-alpha.1",
                "1.0.0-alpha.beta",
                "1.0.0-beta",
                "1.0.0-beta.2",
                "1.0.0-beta.11",
                "1.0.0-rc.1",
                "1.0.0",
            };

            CollectionAssert.AreEqual(exptected, actual);
        }
    }
}
