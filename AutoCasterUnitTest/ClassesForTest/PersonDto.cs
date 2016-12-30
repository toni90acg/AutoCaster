using System.Diagnostics.CodeAnalysis;

namespace AutoCasterUnitTest.ClassesForTest
{
    [ExcludeFromCodeCoverage]
    public class PersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
