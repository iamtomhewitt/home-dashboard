using UnityEngine;
using System.Collections;

/// <summary>
/// A widget that fades between its Run() method.
/// </summary>
public abstract class FadingWidget : Widget
{
	private float fadeInDelay = 0.15f;

	public abstract void FadeIn();
	public abstract void FadeOut();

	/// <summary>
	/// Fade the widget, calling a void method in between.
	/// </summary>
	public IEnumerator Fade(VoidMethodToCallBetweenFading Method, float fadeOutLength)
	{
		FadeOut();

		yield return new WaitForSeconds(fadeOutLength);

		Method();

		yield return new WaitForSeconds(fadeInDelay);

		FadeIn();
	}

	/// <summary>
	/// Fade the widget, calling an IEnumerator method in between.
	/// </summary>
	public IEnumerator Fade(IEnumeratorMethodToCallBetweenFading Method, float fadeOutLength)
	{
		FadeOut();

		yield return new WaitForSeconds(fadeOutLength);

		yield return StartCoroutine(Method());

		yield return new WaitForSeconds(fadeInDelay);

		FadeIn();
	}
}

public delegate void VoidMethodToCallBetweenFading();
public delegate IEnumerator IEnumeratorMethodToCallBetweenFading();
