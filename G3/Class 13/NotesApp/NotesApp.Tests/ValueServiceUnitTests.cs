using NotesApp.Services.Implementation;

namespace NotesApp.Tests
{
    [TestClass]
    public class ValueServiceUnitTests
    {
        private readonly ValuesService _service;

        public ValueServiceUnitTests()
        {
            _service = new ValuesService();
        }

        [TestMethod]
        public void SumPositiveNumbers_NegativeValue_ShouldReturnNull()
        {
            //Arange 
            int num1 = -2; //we are testing the case when se have negative number as input
            int num2 = 2;

            //Act 
            int? result = _service.SumPositiveNumbers(num1, num2); //here we call the method that we want to test with the data for testing

            //Assert 
            Assert.IsNull(result); //here we expect our result to be null, so we check if that is what was returned
        }

        [TestMethod]
        public void SumPositiveNumers_ValidValues_ShouldReturnFive()
        {
            //Arrange
            int num1 = 2;
            int num2 = 3;
            int expectedResult = 5;

            //Act 
            int? result = _service.SumPositiveNumbers(num1, num2);

            //Assert
            Assert.AreEqual(expectedResult, result); //we check if the result we expected is the same as the one that was returned
        }

        [TestMethod]
        public void IsFirstNumberLarger_ShouldReturnTrue()
        {
            //Arange
            int num1 = 5;
            int num2 = 2;

            //Act
            bool result = _service.IsFirstNumberLarger(num1, num2);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsFirstNumberLarger_ShouldReturnFalse()
        {
            //Arange
            int num1 = 5;
            int num2 = 2;

            //Act
            bool result = _service.IsFirstNumberLarger(num2, num1);

            //Assert
            Assert.IsFalse(result);
        }
    }
}
