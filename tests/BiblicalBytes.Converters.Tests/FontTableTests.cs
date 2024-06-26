using BiblicalBytes.Contracts;
using Moq;

namespace BiblicalBytes.Converters.Tests;

/// <summary>
/// Represents testing for a table of fonts, allowing for addition and retrieval of fonts by index.
/// </summary>
public class FontTableTests
{
    private readonly Mock<ILogger> mockLogger;
    private readonly IDictionary<int, string> fonts;
    private readonly FontTable fontTable;

    public FontTableTests()
    {
        mockLogger = new Mock<ILogger>();
        fonts = new Dictionary<int, string>();
        fontTable = new FontTable(fonts, mockLogger.Object);
    }

    [Fact]
    public void Add_WithName_AddsFontWithNewIndex()
    {
        // Arrange
        var fontName = "Arial";

        // Act
        fontTable.Add(fontName);

        // Assert
        Assert.Contains(fonts, pair => pair.Value == fontName);
    }

    [Fact]
    public void Add_WithIndexAndName_AddsFontAtIndex()
    {
        // Arrange
        var fontName = "Arial";
        var index = 1;

        // Act
        fontTable.Add(index, fontName);

        // Assert
        Assert.Equal(fontName, fonts[index]);
    }

    [Fact]
    public void Add_WithExistingIndex_LogsWarning()
    {
        // Arrange
        var fontName = "Arial";
        var index = 1;
        fontTable.Add(index, fontName);

        // Act
        fontTable.Add(index, "Helvetica");

        // Assert
        mockLogger.Verify(logger => logger.Warning(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void Indexer_GetsFontNameByIndex()
    {
        // Arrange
        var fontName = "Arial";
        var index = 1;
        fontTable.Add(index, fontName);

        // Act
        var result = fontTable[index];

        // Assert
        Assert.Equal(fontName, result);
    }

    [Fact]
    public void Count_ReturnsNumberOfFonts()
    {
        // Arrange
        fontTable.Add("Arial");
        fontTable.Add("Helvetica");

        // Act
        var count = fontTable.Count;

        // Assert
        Assert.Equal(2, count);
    }

    [Fact]
    public void IndexOf_WithExistingFont_ReturnsIndex()
    {
        // Arrange
        var fontName = "Arial";
        fontTable.Add(fontName);

        // Act
        var index = fontTable.IndexOf(fontName);

        // Assert
        Assert.True(index >= 0);
    }

    [Fact]
    public void IndexOf_WithNonExistingFont_ReturnsDefaultInt()
    {
        // Arrange
        var fontName = "NonExistingFont";

        // Act
        var index = fontTable.IndexOf(fontName);

        // Assert
        Assert.Equal(default, index);
    }

    [Fact]
    public void Remove_WithIndex_RemovesFont()
    {
        // Arrange
        var fontName = "Arial";
        var index = 1;
        fontTable.Add(index, fontName);

        // Act
        fontTable.Remove(index);

        // Assert
        Assert.DoesNotContain(fonts, pair => pair.Key == index);
    }

    [Fact]
    public void Remove_WithName_RemovesFont()
    {
        // Arrange
        var fontName = "Arial";
        fontTable.Add(fontName);

        // Act
        fontTable.Remove(fontName);

        // Assert
        Assert.DoesNotContain(fonts, pair => pair.Value == fontName);
    }
}