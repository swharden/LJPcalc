using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LJPtest
{
    class IonTableTest
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            //System.IO.File.Copy("../../../")
        }

        [Test]
        public void Test_NoFile_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new LJPmath.IonTable("badFileName.csv"));
        }

        [Test]
        public void Test_EmptyConstructor_IsFine()
        {
            var ionTable = new LJPmath.IonTable();
            Console.WriteLine(ionTable);
        }

        [Test]
        public void Test_ConstructorFilename_IsFine()
        {
            var ionTable = new LJPmath.IonTable("IonTable.csv");
            Console.WriteLine(ionTable);
        }

        [Test]
        public void Test_Lookup_KnownIon()
        {
            var ionTable = new LJPmath.IonTable();
            var ion = ionTable.Lookup("glutamate");
            Console.WriteLine(ion);
            Assert.AreEqual(-1, ion.charge);
        }

        [Test]
        public void Test_Lookup_IsCaseInsensitive()
        {
            var ionTable = new LJPmath.IonTable();
            var ion = ionTable.Lookup("gLuTaMaTe");
            Console.WriteLine(ion);
            Assert.AreEqual(-1, ion.charge);
        }

        [Test]
        public void Test_Lookup_ByDescription()
        {
            var ionTable = new LJPmath.IonTable();
            var ion = ionTable.Lookup("Glutamate");
            Console.WriteLine(ion);
            Assert.AreEqual(-1, ion.charge);
        }

        [Test]
        public void Test_Lookup_FailsWithoutCrashing()
        {
            var ionTable = new LJPmath.IonTable();
            var ion = ionTable.Lookup("adfasdfasdfasdf");
            Console.WriteLine(ion);
            Assert.AreEqual(0, ion.charge);
        }

        [Test]
        public void Test_Lookup_AcceptsNull()
        {
            var ionTable = new LJPmath.IonTable();
            var ion = ionTable.Lookup(null);
            Console.WriteLine(ion);
            Assert.AreEqual(0, ion.charge);
        }
    }
}
