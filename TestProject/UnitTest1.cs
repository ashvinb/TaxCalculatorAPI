using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic;

namespace TaxCalculator.Tests
{
    public class TaxCalculatorTests
    {
        [Theory]
        [InlineData(50000, "7441", 8687.6)]
        [InlineData(250000, "1000", 67642.6)]
        [InlineData(150000, "A100", 7500)]
        [InlineData(220000, "A100", 10000)]
        [InlineData(120000, "7000", 21000)]
        public async Task CalculateTax_ValidInputs_ReturnsCorrectTax(int annualIncome, string postalCode, double expectedTax)
        {
            // Arrange
            BusinessLogic.TaxCalculator taxCalculator = new BusinessLogic.TaxCalculator();
            
            // Act
            double tax = await taxCalculator.CalculateTax(annualIncome, postalCode);

            // Assert
            Assert.Equal(expectedTax, tax, 2);
        }

        [Fact]
        public void CalculateProgressiveTax_ValidInputs_ReturnsCorrectTax()
        {
            // Arrange
            BusinessLogic.TaxCalculator taxCalculator = new BusinessLogic.TaxCalculator();
            int annualIncome = 50000;
            List<TaxBracket> taxBrackets = new List<TaxBracket>
            {
                new TaxBracket { From = 0, To = 8350, Rate = 0.1 },
                new TaxBracket { From = 8351, To = 33950, Rate = 0.15 },
                new TaxBracket { From = 33951, To = 82250, Rate = 0.25 },
                new TaxBracket { From = 82251, To = 171550, Rate = 0.28 },
                new TaxBracket { From = 171551, To = 372950, Rate = 0.33 },
                new TaxBracket { From = 372951, To = 999999999, Rate = 0.35 }
            };

            // Act
            double tax = taxCalculator.CalculateProgressiveTax(annualIncome, taxBrackets);

            // Assert
            Assert.Equal(8687.6, tax, 2);
        }

        [Theory]
        [InlineData(150000, 7500)]
        [InlineData(220000, 10000)]
        public void CalculateFlatValueTax_ValidInputs_ReturnsCorrectTax(int annualIncome, double expectedTax)
        {
            // Arrange
            BusinessLogic.TaxCalculator taxCalculator = new BusinessLogic.TaxCalculator();

            // Act
            double tax = taxCalculator.CalculateFlatValueTax(annualIncome);

            // Assert
            Assert.Equal(expectedTax, tax);
        }

        [Theory]
        [InlineData("InvalidPostalCode")]
        [InlineData("1234")]
        public async Task CalculateTax_InvalidPostalCode_ReturnsZeroTax(string postalCode)
        {
            // Arrange
            BusinessLogic.TaxCalculator taxCalculator = new BusinessLogic.TaxCalculator();
            int annualIncome = 50000;

            // Act
            double tax = await taxCalculator.CalculateTax(annualIncome, postalCode);

            // Assert
            Assert.Equal(0, tax);
        }

    }

}