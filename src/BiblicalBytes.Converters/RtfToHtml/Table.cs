namespace BiblicalBytes.Converters.RtfToHtml;

public class Table
{
    public string RtfReferenceRow;
    public int AmountOfColumns;
    public int DefaultLengthOfPageInTwips;
    public Table()
    {
        this.RtfReferenceRow = "\\clbrdrt\\brdrw15\\brdrs\\clbrdrl\\brdrw15\\brdrs\\clbrdrb\\brdrw15\\brdrs\\clbrdrr\\brdrw15\\brdrs\\cellx";
        this.AmountOfColumns = 0;
        this.DefaultLengthOfPageInTwips = 8503;
    }

    public void SetAmountOfColumns(int amountOfColumns)
    {
        this.AmountOfColumns = amountOfColumns;
    }

    public int GetAmountOfColumns()
    {
        return this.AmountOfColumns;
    }

    public double GetCellLength()
    {
        return Math.Floor((double)(this.DefaultLengthOfPageInTwips / this.AmountOfColumns));
    }

    public string GetRtfReferenceRow()
    {
        return this.RtfReferenceRow;
    }

    public string BuildCellsLengthOfEachColumn()
    {
        var cellGroup = "";
        for (var columnNumber = 0; columnNumber < this.AmountOfColumns; columnNumber++)
        {

            cellGroup += this.RtfReferenceRow + (this.GetCellLength() * columnNumber + this.GetCellLength());

        }
        return cellGroup;
    }
}