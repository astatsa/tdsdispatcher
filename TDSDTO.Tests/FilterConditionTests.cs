using NUnit.Framework;
using TDSDTO.Filter;

namespace TDSDTO.Tests
{
    public class FilterConditionTests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void FieldValueEqualTo5()
        {
            var equalCondition = new FilterCondition<FieldOperand, int>(
                new FieldOperand("Value"),
                5,
                ConditionOperation.Equal
                );

            Assert.AreEqual(true, equalCondition.ExecuteFilter(new { Value = 5 }));
        }
    }
}