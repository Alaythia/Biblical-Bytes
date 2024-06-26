using BiblicalBytes.Contracts;
using BiblicalBytes.Converters.RtfTree;
using Moq;

namespace BiblicalBytes.Converters.Tests;

public class StylesheetTableTests
{
    private readonly Dictionary<int, RtfStyleSheet> stylesheets = new();
    private readonly StylesheetTable stylesheetTable;

    public StylesheetTableTests()
    {
        Mock<ILogger> mockLogger = new();
        stylesheetTable = new StylesheetTable(stylesheets, mockLogger.Object);
    }

    [Fact]
    public void Add_WithStyleSheet_AddsStyleSheetWithNewIndex()
    {
        // Arrange
        var styleSheet = new RtfStyleSheet { Name = "TestStyle" };

        // Act
        stylesheetTable.Add(styleSheet);

        // Assert
        Assert.Contains(stylesheets, pair => pair.Value == styleSheet);
    }

    [Fact]
    public void Add_WithIndexAndStyleSheet_AddsStyleSheetAtIndex()
    {
        // Arrange
        var styleSheet = new RtfStyleSheet { Name = "TestStyle" };
        var index = 1;

        // Act
        stylesheetTable.Add(index, styleSheet);

        // Assert
        Assert.Equal(styleSheet, stylesheets[index]);
    }

    [Fact]
    public void IndexOf_WithExistingStyleSheet_ReturnsIndex()
    {
        // Arrange
        var styleSheet = new RtfStyleSheet { Name = "TestStyle" };
        stylesheetTable.Add(styleSheet);

        // Act
        var index = stylesheetTable.IndexOf(styleSheet);

        // Assert
        Assert.True(index >= 0);
    }

    [Fact]
    public void IndexOf_WithNonExistingStyleSheet_ReturnsDefaultInt()
    {
        // Arrange
        var styleSheet = new RtfStyleSheet { Name = "NonExistingStyle" };

        // Act
        var index = stylesheetTable.IndexOf(styleSheet);

        // Assert
        Assert.Equal(default, index);
    }

    [Fact]
    public void Remove_WithIndex_RemovesStyleSheet()
    {
        // Arrange
        var styleSheet = new RtfStyleSheet { Name = "TestStyle" };
        var index = 1;
        stylesheetTable.Add(index, styleSheet);

        // Act
        stylesheetTable.Remove(index);

        // Assert
        Assert.DoesNotContain(stylesheets, pair => pair.Key == index);
    }

    [Fact]
    public void Remove_WithStyleSheet_RemovesStyleSheet()
    {
        // Arrange
        var styleSheet = new RtfStyleSheet { Name = "TestStyle", Index = 1 };
        stylesheetTable.Add(styleSheet.Index, styleSheet);

        // Act
        stylesheetTable.Remove(styleSheet);

        // Assert
        Assert.DoesNotContain(stylesheets, pair => pair.Value == styleSheet);
    }

    [Fact]
    public void Indexer_GetsStyleSheetByIndex()
    {
        // Arrange
        var styleSheet = new RtfStyleSheet { Name = "TestStyle" };
        var index = 1;
        stylesheetTable.Add(index, styleSheet);

        // Act
        var result = stylesheetTable[index];

        // Assert
        Assert.Equal(styleSheet, result);
    }

    [Fact]
    public void Count_ReturnsNumberOfStyleSheets()
    {
        // Arrange
        stylesheetTable.Add(new RtfStyleSheet { Name = "Style1" });
        stylesheetTable.Add(new RtfStyleSheet { Name = "Style2" });

        // Act
        var count = stylesheetTable.Count;

        // Assert
        Assert.Equal(2, count);
    }

    [Fact]
    public void IndexOf_WithName_ReturnsIndex()
    {
        // Arrange
        var styleSheet = new RtfStyleSheet { Name = "TestStyle" };
        stylesheetTable.Add(styleSheet);

        // Act
        var index = stylesheetTable.IndexOf("TestStyle");

        // Assert
        Assert.True(index >= 0);
    }

    [Fact]
    public void IndexOf_WithNonExistingName_ReturnsDefaultInt()
    {
        // Arrange
        var name = "NonExistingStyle";

        // Act
        var index = stylesheetTable.IndexOf(name);

        // Assert
        Assert.Equal(default, index);
    }
}