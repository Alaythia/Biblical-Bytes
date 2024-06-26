using System.Drawing;
using BiblicalBytes.Contracts;
using Moq;

namespace BiblicalBytes.Converters.Tests;

public class ColorTableTests
{
    private readonly Mock<ILogger> mockLogger;
    private readonly Dictionary<int, Color> colors;
    private ColorTable colorTable;

    public ColorTableTests()
    {
        mockLogger = new Mock<ILogger>();
        colors = new Dictionary<int, Color>();
        colorTable = new ColorTable(colors, mockLogger.Object);
    }

    [Fact]
    public void Add_WithColor_AddsColorWithNewIndex()
    {
        // Arrange
        var color = Color.Red;

        // Act
        colorTable.Add(color);

        // Assert
        Assert.Contains(colors, pair => pair.Value == color);
    }

    [Fact]
    public void Add_WithIndexAndColor_AddsColorAtIndex()
    {
        // Arrange
        var color = Color.Blue;
        var index = 1;

        // Act
        colorTable.Add(index, color);

        // Assert
        Assert.Equal(color, colors[index]);
    }

    [Fact]
    public void Add_WithExistingIndex_LogsWarning()
    {
        // Arrange
        var color = Color.Green;
        var index = 1;
        colorTable.Add(index, color);

        // Act
        colorTable.Add(index, Color.Yellow);

        // Assert
        mockLogger.Verify(logger => logger.Warning(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void Indexer_GetsColorByIndex()
    {
        // Arrange
        var color = Color.Purple;
        var index = 2;
        colorTable.Add(index, color);

        // Act
        var result = colorTable[index];

        // Assert
        Assert.Equal(color, result);
    }

    [Fact]
    public void Count_ReturnsNumberOfColors()
    {
        // Arrange
        colorTable.Add(Color.Red);
        colorTable.Add(Color.Blue);

        // Act
        var count = colorTable.Count;

        // Assert
        Assert.Equal(2, count);
    }

    [Fact]
    public void IndexOf_WithExistingColor_ReturnsIndex()
    {
        // Arrange
        var color = Color.Red;
        colorTable.Add(color);

        // Act
        var index = colorTable.IndexOf(color);

        // Assert
        Assert.True(index >= 0);
    }

    [Fact]
    public void IndexOf_WithNonExistingColor_ReturnsMinusOne_OnEmptyTable()
    {
        // Arrange
        var nonExistingColor = Color.Red;
        
        // Act
        var index = colorTable.IndexOf(nonExistingColor);

        // Assert
        Assert.Equal(-1, index);
    }

    [Fact]
    public void IndexOf_WithNonExistingColor_ReturnsMinusOne()
    {
        // Arrange
        var color = Color.Blue;
        var nonExistingColor = Color.Red;
        colorTable.Add(color);

        // Act
        var index = colorTable.IndexOf(nonExistingColor);

        // Assert
        Assert.Equal(-1, index);
    }

    [Fact]
    public void IndexOf_WithNonExistingColorByName_ReturnsMinusOne_OnEmptyTable()
    {
        // Arrange
        var nonExistingColor = Color.Red;

        // Act
        var index = colorTable.IndexOf(nonExistingColor.Name);

        // Assert
        Assert.Equal(-1, index);
    }

    [Fact]
    public void IndexOf_WithNonExistingColorByName_ReturnsMinusOne()
    {
        // Arrange
        var color = Color.Blue;
        var nonExistingColor = Color.Red;
        colorTable.Add(color);

        // Act
        var index = colorTable.IndexOf(nonExistingColor.Name);

        // Assert
        Assert.Equal(-1, index);
    }

    [Fact]
    public void Remove_WithIndex_RemovesColor()
    {
        // Arrange
        var color = Color.Red;
        var index = 1;
        colorTable.Add(index, color);

        // Act
        colorTable.Remove(index);

        // Assert
        Assert.DoesNotContain(colors, pair => pair.Key == index);
    }

    [Fact]
    public void Remove_WithColor_RemovesColor()
    {
        // Arrange
        var color = Color.Red;
        colorTable.Add(color);

        // Act
        colorTable.Remove(color);

        // Assert
        Assert.DoesNotContain(colors, pair => pair.Value == color);
    }
}