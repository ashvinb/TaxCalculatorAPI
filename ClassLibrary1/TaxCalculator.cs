using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dapper;

namespace BusinessLogic
{
    public class TaxCalculator : ITaxCalculator
    {
        public Task<double> CalculateTax(int annualIncome, string postalCode)
        {
            double tax = 0.0;

            if (postalCode == "7441" || postalCode == "1000")
            {

                //var taxBrackets = GetTaxBrackets();

                List<TaxBracket> taxBrackets = new List<TaxBracket>
                {
                    new TaxBracket { From = 0, To = 8350, Rate = 0.1 },
                    new TaxBracket { From = 8351, To = 33950, Rate = 0.15 },
                    new TaxBracket { From = 33951, To = 82250, Rate = 0.25 },
                    new TaxBracket { From = 82251, To = 171550, Rate = 0.28 },
                    new TaxBracket { From = 171551, To = 372950, Rate = 0.33 },
                    new TaxBracket { From = 372951, To = 999999999, Rate = 0.35 }
                };

                tax = CalculateProgressiveTax(annualIncome, taxBrackets);

            }
            else if (postalCode == "A100")
            {
                tax = CalculateFlatValueTax(annualIncome);
            }
            else if (postalCode == "7000")
            {
                tax = CalculateFlatRateTax(annualIncome);
            }
            else
            {
                Console.WriteLine("Invalid postal code.");
            }

            return Task.FromResult(tax);
        }

        public double CalculateProgressiveTax(int annualIncome, List<TaxBracket> taxBrackets)
        {
            double taxOwed = 0;

            foreach (var bracket in taxBrackets)
            {
                if (annualIncome <= bracket.To)
                {
                    taxOwed += (annualIncome - bracket.From + 1) * bracket.Rate;
                    break;
                }
                else
                {
                    taxOwed += (bracket.To - bracket.From + 1) * bracket.Rate;
                }
            }

            return taxOwed;
        }

        public double CalculateFlatValueTax(int annualIncome)
        {
            return (annualIncome < 200000) ? annualIncome * 0.05 : 10000;
        }

        public double CalculateFlatRateTax(int annualIncome)
        {
            return annualIncome * 0.175;
        }

        public async Task<List<TaxBracket>> GetTaxBrackets()
        {
            using (SqlConnection connection = new SqlConnection("your_connection_string"))
            {
                connection.Open();

                string sql = "SELECT [From], [To], [Rate] FROM [TaxBracket]";
                var taxBrackets = await connection.QueryAsync<TaxBracket>(sql);

                return taxBrackets.ToList();
            }
        }
    }

    public class TaxBracket
    {
        public int From { get; set; }
        public int To { get; set; }
        public double Rate { get; set; }
    }
}
