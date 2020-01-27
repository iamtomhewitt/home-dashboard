using UnityEngine;

public class Colours
{
	public static Color Lighten(Color colour)
	{
		return Lighten(colour, 0.3f);
	}

	public static Color Lighten(Color colour, float amount)
	{
		return Color.Lerp(colour, Color.white, amount);
	}

	public static Color Darken(Color colour)
	{
		return Color.Lerp(colour, Color.black, 0.3f);
	}

	public static Color ToColour(string hex)
	{
		Color colour;

		if (ColorUtility.TryParseHtmlString(hex, out colour))
		{
            return colour;
		}
		else
		{
			return Color.white;
		}
	}
}