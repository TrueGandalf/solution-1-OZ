using TerminalLibrary;

namespace TerminalLibraryTests;


[Collection("SequentialTests")]
public class CalculateTotalTests
{
    //Scan these items in this order: AAAABCDAAA; Verify the total price is $13.25.
    //Scan these items in this order: CCCCCCC; Verify the total price is $6.00.
    //Scan these items in this order: ABCD; Verify the total price is $7.25

    [Theory]
    [InlineData("AAAABCDAAA", 13.25)]
    [InlineData("CCCCCCC", 6.00)]
    [InlineData("ABCD", 7.25)]
    public void CalculateTotalReturnsCorrectAmmount(string input, double expectedResult)
    {
        // Arrange
        PointOfSaleTerminal terminal = new PointOfSaleTerminal();

        terminal.SetPricing("A", volume: 1, price: 1.25);
        terminal.SetPricing("A", volume: 3, price: 3.00);
        terminal.SetPricing("B", volume: 1, price: 4.25);
        terminal.SetPricing("C", volume: 1, price: 1.00);
        terminal.SetPricing("C", volume: 6, price: 5.00);
        terminal.SetPricing("D", volume: 1, price: 0.75);

        // Act
        terminal.ScanSplitted(input);
        var result = terminal.CalculateTotal();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}