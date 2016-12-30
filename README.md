
# AutoCaster
The AutoCaster is a tool to cast everything.

It can cast simple types and can map complex objects by itself (the only condition is that the two objects has to have the properties with same name).

For custom mappings, you can register new mapping funcs.

Really simple to use, look at the Test Classes.


As simple as:

            var person = new Person()
            {
                Id = 1,
                Name = "Toni",
                Age = 26
            };

            var personDto = new AutoCaster()
                               .AutoCast<PersonDto>(person);
                            
       or use the Singleton AutoCaster
       
            var personDto = AutoCasterSingleton
                               .GetInstance()
                               .AutoCast<PersonDto>(person);

If you need an specific mapping for an object:

            var personDto = new AutoCaster()
                               .RegisterCastMapping<PersonDto>(p =>
                                {
                                    var per = p as Person;
                                    if (per == null) return null;
                                    return new PersonDto()
                                    {
                                       Id = per.Id,
                                       Name = per.Name,
                                       Age = per.Age
                                    };
                                })
                               .AutoCast<PersonDto>(person);
                               
      or use the Singleton AutoCaster
       
            var personDto = AutoCasterSingleton
                               .GetInstance()
                               .RegisterCastMapping<PersonDto>(p =>
                                {
                                    var per = p as Person;
                                    if (per == null) return null;
                                    return new PersonDto()
                                    {
                                       Id = per.Id,
                                       Name = per.Name,
                                       Age = per.Age
                                    };
                                })
                               .AutoCast<PersonDto>(person);


More information:
It uses Strategy Pattern, Singleton Pattern and Fluent Interface.
