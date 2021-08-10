using System;
using alterNERDtive.Edna.Edts;
using Xunit;

namespace Test.Edts
{
    public class LocationTest
    {
        [Theory]
        [InlineData("Oevasy SG-Y D0")]
        public void ProcGen_Valid(string name)
        {
            EdtsApi.Find_System(name: name);
        }

        [Theory]
        [InlineData("just a test name")]
        public void ProcGen_Invalid(string name)
        {
            Assert.Throws<ArgumentException>(() => EdtsApi.Find_System(name: name));
        }
    }
}
