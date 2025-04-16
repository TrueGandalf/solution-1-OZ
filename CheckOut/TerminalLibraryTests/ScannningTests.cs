using TerminalLibrary;

namespace TerminalLibraryTests;

[Collection("SequentialTests")]
public class ScannningTests
{
    // Scan these items in this order: AAAABCDAAA; Expected quantities for each product are A: 7, B: 1, C: 1, D: 1
    // Scan these items in this order: CCCCCCC; Expected quantities for each product are C: 7
    // Scan these items in this order: ABCD; Expected quantities for each product are A: 1, B: 1, C: 1, D: 1


    [Theory]
    [InlineData("AAAABCDAAA", "A", 7, "B", 1, "C", 1, "D", 1)]  // Test case 1
    [InlineData("CCCCCCC", "C", 7)]                              // Test case 2
    [InlineData("ABCD", "A", 1, "B", 1, "C", 1, "D", 1)]        // Test case 3
    public void ScanningReturnsCorrectQtts(string input, params object[] expectedResult)
    {
        // Arrange
        PointOfSaleTerminal terminal = new PointOfSaleTerminal();

        // Act
        terminal.ScanSplitted(input);

        var expectedDictionary = new Dictionary<string, int>();
        for (int i = 0; i < expectedResult.Length; i += 2)
        {
            expectedDictionary.Add((string)expectedResult[i], (int)expectedResult[i + 1]);
        }

        // Assert
        var actualDictionary = new Dictionary<string, int>();
        foreach (var product in terminal.Busket.Keys)
        {
            actualDictionary[product.Name] = terminal.Busket[product];
        }

        Assert.Equal(expectedDictionary, actualDictionary);
    }
}
