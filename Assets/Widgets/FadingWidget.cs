using UnityEngine;
using System.Collections;

/// <summary>
/// A widget that fades between its Run() method.
/// </summary>
public abstract class FadingWidget : Widget
{
	[Header("Fading Settings")]
	[SerializeField] private CanvasGroup canvasGroup;

	[SerializeField] private float fadeSpeed = 2f;
	[SerializeField] private float fadeInDelay = 0.15f;

	private IEnumerator FadeIn()
	{
		canvasGroup.alpha = 0f;

		while (canvasGroup.alpha < 1)
		{
			canvasGroup.alpha += Time.deltaTime * fadeSpeed;
			yield return null;
		}

		yield return null;
	}

	private IEnumerator FadeOut()
	{
		canvasGroup.alpha = 1f;

		while (canvasGroup.alpha > 0)
		{
			canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
			yield return null;
		}

		yield return null;
	}

	/// <summary>
	/// Fade the widget, calling a void method in between.
	/// </summary>
	public IEnumerator Fade(VoidMethodToCallBetweenFading Method, float fadeOutLength)
	{
		StartCoroutine(FadeOut());

		yield return new WaitForSeconds(fadeOutLength);

		Method();

		yield return new WaitForSeconds(fadeInDelay);

		StartCoroutine(FadeIn());
	}

	/// <summary>
	/// Fade the widget, calling an IEnumerator method in between.
	/// </summary>
	public IEnumerator Fade(IEnumeratorMethodToCallBetweenFading Method, float fadeOutLength)
	{
		StartCoroutine(FadeOut());

		yield return new WaitForSeconds(fadeOutLength);

		yield return StartCoroutine(Method());

		yield return new WaitForSeconds(fadeInDelay);

		StartCoroutine(FadeIn());
	}
}

public delegate void VoidMethodToCallBetweenFading();
public delegate IEnumerator IEnumeratorMethodToCallBetweenFading();
