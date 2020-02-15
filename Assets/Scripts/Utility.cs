using System.Text.RegularExpressions;

public class Utility
{
	public static string CamelCaseToSentence(string camelCase)
	{
		// Capitalise first letter
		camelCase = char.ToUpper(camelCase[0]) + camelCase.Substring(1);

		// Now split into words
		Regex r = new Regex(@"(?<=[A-Z])(?=[A-Z][a-z]) | (?<=[^A-Z])(?=[A-Z]) | (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
		return r.Replace(camelCase, " ");
	}
}
