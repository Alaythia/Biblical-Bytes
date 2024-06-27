namespace BiblicalBytes.Converters.RtfToHtml;

/// <summary>
/// Represents a table in RTF format and provides methods to manipulate and convert it to HTML.
/// </summary>
public class Table
{
    /// <summary>
    /// Gets or sets the RTF reference row that defines the table's border and cell properties.
    /// </summary>
    public string RtfReferenceRow { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the amount of columns in the table.
    /// </summary>
    public int AmountOfColumns { get; set; }

    /// <summary>
    /// Gets or sets the default length of the page in twips. A twip is a unit of measurement used in typography.
    /// </summary>
    public int DefaultLengthOfPageInTwips { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Table"/> class with default settings.
    /// </summary>
    public Table()
    {
        RtfReferenceRow = "\\clbrdrt\\brdrw15\\brdrs\\clbrdrl\\brdrw15\\brdrs\\clbrdrb\\brdrw15\\brdrs\\clbrdrr\\brdrw15\\brdrs\\cellx";
        AmountOfColumns = 0;
        DefaultLengthOfPageInTwips = 8503;
    }

    /// <summary>
    /// Sets the amount of columns in the table.
    /// </summary>
    /// <param name="amountOfColumns">The amount of columns.</param>
    public void SetAmountOfColumns(int amountOfColumns)
    {
        AmountOfColumns = amountOfColumns;
    }

    /// <summary>
    /// Gets the amount of columns in the table.
    /// </summary>
    /// <returns>The amount of columns.</returns>
    public int GetAmountOfColumns()
    {
        return AmountOfColumns;
    }

    /// <summary>
    /// Calculates the length of each cell based on the total amount of columns and the default page length.
    /// </summary>
    /// <returns>The length of a single cell in twips.</returns>
    public double GetCellLength()
    {
        return AmountOfColumns == 0 ? 0 : Math.Floor((double)(DefaultLengthOfPageInTwips / AmountOfColumns));
    }

    /// <summary>
    /// Gets the RTF reference row.
    /// </summary>
    /// <returns>The RTF reference row.</returns>
    public string GetRtfReferenceRow()
    {
        return RtfReferenceRow;
    }

    /// <summary>
    /// Builds the RTF code for the length of each column in the table.
    /// </summary>
    /// <returns>A string containing the RTF code for the cell lengths of each column.</returns>
    public string BuildCellsLengthOfEachColumn()
    {
        var cellGroup = "";
        for (var columnNumber = 0; columnNumber < AmountOfColumns; columnNumber++)
        {
            cellGroup += RtfReferenceRow + (GetCellLength() * columnNumber + GetCellLength());
        }
        return cellGroup;
    }
}
