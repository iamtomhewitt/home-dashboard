using UnityEngine;
using System.Collections;

/// <summary>
/// A fading widget that uses the CanvasGroup for its fading.
/// </summary>
public abstract class CanvasGroupFadingWidget : FadingWidget
{
	[Space(15f)]
	[SerializeField] private CanvasGroup canvasGroup;

	private float fadeSpeed = 2f;

	public override void FadeIn()
	{
		StartCoroutine(FadeInRoutine());
	}

	public override void FadeOut()
	{
		StartCoroutine(FadeOutRoutine());
	}

	private IEnumerator FadeInRoutine()
	{
		canvasGroup.alpha = 0f;

		while (canvasGroup.alpha < 1)
		{
			canvasGroup.alpha += Time.deltaTime * fadeSpeed;
			yield return null;
		}

		canvasGroup.interactable = false;
		yield return null;
	}

	private IEnumerator FadeOutRoutine()
	{
		canvasGroup.alpha = 1f;

		while (canvasGroup.alpha > 0)
		{
			canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
			yield return null;
		}

		canvasGroup.interactable = false;
		yield return null;
	}
}
