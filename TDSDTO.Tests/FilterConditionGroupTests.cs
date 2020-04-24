using NUnit.Framework;
using TDSDTO.Filter;

namespace TDSDTO.Tests
{
    class FilterConditionGroupTests
    {
        [Test]
        public void ValueMoreThan3AndNameEqualTest()
        {
            FilterConditionGroup gr = new FilterConditionGroup
            {
                Conditions = new FilterConditionCollection
                {
                    new FilterCondition<FieldOperand, int>(new FieldOperand("Value"), 3, ConditionOperation.Greater),
                    new FilterCondition<FieldOperand, string>(new FieldOperand("Name"), "Test", ConditionOperation.Equal)
                }
            };

            Assert.IsTrue(gr.ExecuteFilter(new { Name = "Test", Value = 10 }));
        }

        [Test]
        public void Test()
        {
            var fo = new FieldOperand("Value");
            var foVal2 = new FieldOperand("Value2");

            FilterConditionGroup gr = new FilterConditionGroup
            {
                Conditions = new FilterConditionCollection
                {
                    new FilterConditionGroup
                    {
                        Operation = ConditionGroupOperation.Or,
                        Conditions = new FilterConditionCollection
                        {
                            new FilterCondition<int, int>(2, 1, ConditionOperation.Equal),
                            new FilterCondition<FieldOperand, int>(fo, 2, ConditionOperation.Equal)
                        }
                    },
                    new FilterConditionGroup
                    {
                        Operation = ConditionGroupOperation.Or,
                        Conditions = new FilterConditionCollection
                        {
                            new FilterCondition<FieldOperand, int>(foVal2, 3, ConditionOperation.Equal),
                            new FilterCondition<FieldOperand, int>(foVal2, 4, ConditionOperation.Equal)
                        }
                    }
                }
            };

            Assert.IsTrue(gr.ExecuteFilter(new { Value = 2, Value2 = 3 }));
        }
    }
}
