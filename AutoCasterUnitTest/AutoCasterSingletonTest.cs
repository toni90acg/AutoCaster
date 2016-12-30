using AutoCaster;
using AutoCaster.Interfaces;
using AutoCasterUnitTest.ClassesForTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoCasterUnitTest
{
    [TestClass]
    public class AutoCasterSingletonTest
    {
        private IAutoCaster _autoCaster;

        [TestMethod]
        [TestCategory("AutoCasterSingleton")]
        public void TestCastingFromStringToDouble()
        {
            _autoCaster = AutoCasterSingleton.GetInstance();

            _autoCaster
                .RegisterCastMapping<Person>(p => p)
                .RegisterCastMapping(typeof(PersonDto), p => p);

            AutoCasterSingleton
                .GetInstance()
                .UnregisterCastMapping<PersonDto>()
                .UnregisterCastMapping(typeof(Person))
                .GetListOfTypesRegistered();

            AutoCasterSingleton
                .GetInstance()
                .RegisterCastMapping<Car>(c => c)
                .RegisterCastMapping<PersonStrings>(p => p);

            var numberOfRegisteredTypesOne = _autoCaster
                .GetListOfTypesRegistered()
                .Count;

            var numberOfRegisteredTypesTwo = AutoCasterSingleton
                .GetInstance()
                .GetListOfTypesRegistered()
                .Count;

            Assert.AreEqual(numberOfRegisteredTypesOne, numberOfRegisteredTypesTwo);
            Assert.AreSame(AutoCasterSingleton.GetInstance(), _autoCaster);
        }
    }
}
