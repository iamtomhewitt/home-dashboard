using UnityEngine;
using System.Collections;

public abstract class FadingWidget : Widget
{
	private float fadeInDelay = 0.15f;

	public abstract void FadeIn();
	public abstract void FadeOut();

	public IEnumerator Fade(MethodToCallBetweenFading Method, float fadeOutLength)
	{
		FadeOut();

		yield return new WaitForSeconds(fadeOutLength);

		Method();

		yield return new WaitForSeconds(fadeInDelay);

		FadeIn();
	}
}

public delegate void MethodToCallBetweenFading();
