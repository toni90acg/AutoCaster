using AutoCaster.Exceptions;
using AutoCaster.Interfaces;
using AutoCasterUnitTest.ClassesForTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoCasterUnitTest
{
    [TestClass]
    public class AutoCasterTest
    {
        private IAutoCaster _autoCaster;

        #region Simple Cast Test Methods

        [TestMethod]
        [TestCategory("AutoCasterSimpleCast")]
        public void TestCastingFromStringToDouble()
        {
            _autoCaster = new AutoCaster.AutoCaster();
            object castedElement;
            var isOkey = _autoCaster.TryAutoCast("2", typeof(double), out castedElement);

            Assert.IsTrue(isOkey);
            Assert.AreEqual(typeof(double), castedElement.GetType());
        }

        [TestMethod]
        [TestCategory("AutoCasterSimpleCast")]
        public void TestCastingFromIntToString()
        {
            _autoCaster = new AutoCaster.AutoCaster();
            object castedElement;

            var isOkey = _autoCaster.TryAutoCast(2, typeof(string), out castedElement);

            Assert.IsTrue(isOkey);
            Assert.AreEqual(typeof(string), castedElement.GetType());
        }

        [TestMethod]
        [TestCategory("AutoCasterSimpleCast")]
        public void TestCastingFromIntToDouble()
        {
            _autoCaster = new AutoCaster.AutoCaster();
            object castedElement;
            var isOkey = _autoCaster.TryAutoCast(2, typeof(double), out castedElement);

            Assert.IsTrue(isOkey);
            Assert.AreEqual(typeof(double), castedElement.GetType());
        }

        [TestMethod]
        [TestCategory("AutoCasterSimpleCast")]
        public void TestCastingFromStringToDoubleWithError()
        {
            _autoCaster = new AutoCaster.AutoCaster();

            object castedElement;
            var isOkey = _autoCaster.TryAutoCast("hola", typeof(double), out castedElement);

            Assert.IsFalse(isOkey);
        }

        #endregion

        #region Mapping Funcs Samples

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

        #endregion

        #region AutoCast With Manual Mapping Funcs

        [TestMethod]
        [TestCategory("AutoCasterWithManualMapping")]
        public void TestAutoCasterSingletonFromPersonToPersonDto()
        {
            _autoCaster = new AutoCaster.AutoCaster()
                .RegisterCastMapping(typeof(PersonDto), MappingFuncFromPersonToPersonDto)
                .RegisterCastMapping(typeof(Person), MappingFuncFromPersonDtoToPerson);

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
            Assert.AreEqual(person.Id, personDto.Id);
            Assert.AreEqual(person.Age, personDto.Age);
            Assert.AreEqual(person.Name, personDto.Name);
        }

        [TestMethod]
        [TestCategory("AutoCasterWithManualMapping")]
        public void TestAutoCasterFromPersonDtoToPerson()
        {
            _autoCaster = new AutoCaster.AutoCaster()
                .RegisterCastMapping(typeof(PersonDto), MappingFuncFromPersonToPersonDto)
                .RegisterCastMapping(typeof(Person), MappingFuncFromPersonDtoToPerson);

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

        #endregion

        #region AutoCast With Auto Mapping

        [TestMethod]
        [TestCategory("AutoCasterWithAutoMapping")]
        public void TestAutoCasterFromPersonToPersonDtoAutoMapping()
        {
            var person = new Person()
            {
                Id = 1,
                Name = "John",
                Age = 26
            };

            _autoCaster = new AutoCaster.AutoCaster();

            var personDto = _autoCaster.AutoCast<PersonDto>(person);

            Assert.AreEqual(typeof(PersonDto), personDto.GetType());
            Assert.AreEqual(personDto.Id, person.Id);
            Assert.AreEqual(personDto.Age, person.Age);
            Assert.AreEqual(personDto.Name, person.Name);
        }

        [TestMethod]
        [TestCategory("AutoCasterWithAutoMapping")]
        public void TestAutoCasterFromPersonStringsToPersonAutoMapping()
        {
            var personStrings = new PersonStrings()
            {
                Id = "1",
                Name = "John",
                Age = "26",
                Addess = "Spain",
                Car = "Porsche 911",
                Height = "182,5",
                Weigth = "65,5",
                NumChildrens = "2"
            };

            _autoCaster = new AutoCaster.AutoCaster();

            var person = _autoCaster.AutoCast<Person>(personStrings);

            Assert.AreEqual(typeof(Person), person.GetType());
            Assert.AreEqual(1, person.Id);
            Assert.AreEqual(26, person.Age);
            Assert.AreEqual(personStrings.Name, person.Name);
            Assert.AreEqual(personStrings.Addess, person.Addess);
            Assert.AreEqual(182.5, person.Height);
            Assert.AreEqual(65.5, person.Weigth);
            Assert.AreEqual(2, person.NumChildrens);
        }

        #endregion

        #region AutoCast Registrations And Unregistrations

        [TestMethod]
        [TestCategory("AutoCasterRegistrationsAndUnregistrations")]
        public void TestAutoCasterRegistration1()
        {
            _autoCaster = new AutoCaster.AutoCaster();
            var typesList = _autoCaster.GetListOfTypesRegistered();
            var numTypesRegisteredBefore = typesList.Count;

            _autoCaster.RegisterCastMapping<PersonDto>(o =>
            {
                if (o.GetType() == typeof(Person))
                {
                    var person = o as Person;
                    if (person == null) return null;
                    return new PersonDto()
                    {
                        Id = person.Id,
                        Age = person.Age,
                        Name = person.Name
                    };
                }
                else
                {
                    return null;
                }
            });

            typesList = _autoCaster.GetListOfTypesRegistered();
            var numTypesRegisteredAfter = typesList.Count;

            var func = _autoCaster.GetFuncForType(typeof(PersonDto));

            Assert.AreEqual(numTypesRegisteredBefore + 1, numTypesRegisteredAfter);
            Assert.IsNotNull(func);
        }

        [TestMethod]
        [TestCategory("AutoCasterRegistrationsAndUnregistrations")]
        public void TestAutoCasterUnregistration1()
        {
            _autoCaster = new AutoCaster.AutoCaster();
            var typesList = _autoCaster.GetListOfTypesRegistered();
            var numTypesRegisteredBefore = typesList.Count;

            _autoCaster.UnregisterCastMapping(typeof(int));

            typesList = _autoCaster.GetListOfTypesRegistered();
            var numTypesRegisteredAfter = typesList.Count;

            Assert.AreEqual(numTypesRegisteredBefore - 1, numTypesRegisteredAfter);
        }

        [TestMethod]
        [TestCategory("AutoCasterRegistrationsAndUnregistrations")]
        public void TestAutoCasterRegistrationForced()
        {
            var person = new Person()
            {
                Id = 1,
                Name = "Antonio",
                Age = 26
            };

            _autoCaster = new AutoCaster.AutoCaster();

            _autoCaster.RegisterCastMapping<PersonDto>(p =>
            {
                var per = p as Person;
                if (per == null) return null;
                return new PersonDto()
                {
                    Name = per.Name
                };
            });

            var personDtoOnlyName = _autoCaster.AutoCast<PersonDto>(person);


            _autoCaster.RegisterCastMappingForced<PersonDto>(p =>
            {
                var per = p as Person;
                if (per == null) return null;
                return new PersonDto()
                {
                    Id = per.Id,
                    Name = per.Name,
                    Age = per.Age
                };
            });

            var personDtoCompleted = _autoCaster.AutoCast<PersonDto>(person);

            Assert.AreEqual(0, personDtoOnlyName.Id);
            Assert.AreEqual(0, personDtoOnlyName.Age);
            Assert.AreEqual(person.Id, personDtoCompleted.Id);
            Assert.AreEqual(person.Age, personDtoCompleted.Age);
        }

        [TestMethod]
        [TestCategory("AutoCasterRegistrationsAndUnregistrations")]
        [ExpectedException(typeof(AutoCasterTypeRegisteredException))]
        public void TestAutoCasterRegistrationException()
        {
            _autoCaster = new AutoCaster.AutoCaster();

            _autoCaster
                .RegisterCastMapping<PersonDto>(p => p)
                .RegisterCastMapping<PersonStrings>(p => p)
                .RegisterCastMapping<PersonDto>(p => p)
                .RegisterCastMapping<Car>(c => c);
        }

        #endregion
    }
}