using AutoCaster;
using AutoCasterUnitTest.ClassesForTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace AutoCasterUnitTest
{
    [TestClass]
    public class AutoCasterSingletonTest
    {
        private AutoCasterSingleton _autoCaster;

        [SetUp]
        public void TestSetup()
        {
            _autoCaster = AutoCasterSingleton.GetAutoCasterInstance();

            _autoCaster.RegisterCastMapping(typeof(PersonDto), MappingFuncFromPersonToPersonDto)
                .RegisterCastMapping(typeof(Person), MappingFuncFromPersonDtoToPerson);
        }

        private object MappingFuncFromPersonToPersonDto(object o)
        {
            var person = o as Person;

            if (person == null)
            {
                return null;
            }

            return new PersonDto()
            {
                Id = person.Id,
                Age = person.Age,
                Name = person.Name
            };
        }

        private object MappingFuncFromPersonDtoToPerson(object o)
        {
            var person = o as PersonDto;

            if (person == null)
            {
                return null;
            }

            return new Person()
            {
                Id = person.Id,
                Age = person.Age,
                Name = person.Name
            };
        }


        [TestMethod]
        [TestCategory("AutoCasterSingletonWithManualMapping")]
        public void TestAutoCasterSingletonFromPersonToPersonDto()
        {
            TestSetup();

            var person = new Person()
            {
                Id = 1,
                Name = "John",
                Age = 26,
                Height = 182.5,
                Weigth = 65.5,
                Addess = "Spain",
                NumChildrens = 2,
                Car = new Car()
                {
                    Brand = "Porsche",
                    Model = "911"
                }
            };

            var personDto = _autoCaster.AutoCast<PersonDto>(person);

            Assert.AreEqual(typeof(PersonDto), personDto.GetType());
            Assert.AreEqual(person.Id,personDto.Id);
            Assert.AreEqual(person.Age, personDto.Age);
            Assert.AreEqual(person.Name, personDto.Name);
        }

        [TestMethod]
        [TestCategory("AutoCasterSingletonWithManualMapping")]
        public void TestAutoCasterSingletonFromPersonDtoToPerson()
        {
            TestSetup();

            var personDto = new PersonDto()
            {
                Id = 1,
                Name = "John",
                Age = 26
            };

            var person = _autoCaster.AutoCast<Person>(personDto);

            Assert.AreEqual(typeof(Person), person.GetType());
            Assert.AreEqual(personDto.Id, person.Id);
            Assert.AreEqual(personDto.Age, person.Age);
            Assert.AreEqual(personDto.Name, person.Name);
        }
    }
}