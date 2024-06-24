namespace BiblicalBytes.Converters.RtfToHtml;

public static class MyString
{
    public static string RemoveCharacterOfEscapeInAllString(string stringValue, string stringOfEscape)
    {
        var listOfEscape = RemoveCharacterOfEscapeNotAllowed(stringOfEscape);
        var newstringValue = "";

        if (listOfEscape == String.Empty)
            return stringValue;
        foreach(var element in stringValue)
        { 
            if (!listOfEscape.Contains(element))
                newstringValue += element;
        }
        return newstringValue;
    }

    public static int convertOneCharInHexToDec(string value)
    {

        if (value.Length != 1)
            return 0;
        if (value[0] >= '0' && value[0] <= '9')
            return value[0];

        else if (Char.ToUpper(value[0]) >= 'A' && Char.ToUpper(value[0]) <= 'F')
        {
            value = Char.ToUpper(value[0]).ToString();
            var number = 0;
            switch (value)
            {
                case "A": number = 10; break;
                case "B": number = 11; break;
                case "C": number = 12; break;
                case "D": number = 13; break;
                case "E": number = 14; break;
                case "F": number = 15; break;
            }
            return number;
        }
        else
            return 0;
    }

    private static string RemoveCharacterOfEscapeNotAllowed(string stringOfEscape)
    {
        string[] listOfCharacterOfEscape = ["\n", "\r", "\t", "\f"];
        var newStringOfEscape = "";
        foreach(var s in listOfCharacterOfEscape)
        { 
            if (stringOfEscape.Contains(s))
                newStringOfEscape+=s;
        }
        return newStringOfEscape.Length > 0 ? newStringOfEscape : null;
    }

    public static bool hasOnlyWhiteSpace(string content)
    {
        return String.IsNullOrWhiteSpace(content);
    }
}