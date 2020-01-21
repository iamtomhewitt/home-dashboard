using UnityEngine;

public class Utils
{
	public static Color Lighten(Color colour)
	{
		return Color.Lerp(colour, Color.white, 0.3f);
	}

	public static Color Lighten(Color colour, float amount)
	{
		return Color.Lerp(colour, Color.white, amount);
	}

	public static Color Darken(Color colour)
	{
		return Color.Lerp(colour, Color.black, 0.3f);
	}
}