namespace BusinessLogic
{
    public interface ITaxCalculator
    {
        double CalculateFlatRateTax(int annualIncome);
        double CalculateFlatValueTax(int annualIncome);
        double CalculateProgressiveTax(int annualIncome, List<TaxBracket> taxBrackets);
        Task<double> CalculateTax(int annualIncome, string postalCode);
        Task<List<TaxBracket>> GetTaxBrackets();
    }
}