using System.Collections;
using UnityEngine;

public class WidgetRotator : MonoBehaviour
{
	[Header("Rotating Settings")]
	[SerializeField] private CanvasGroup widgetOneCanvasGroup;
	[SerializeField] private CanvasGroup widgetTwoCanvasGroup;

	[SerializeField] private float timeBetweenRotation = 30f;
	[SerializeField] private float fadeSpeed = 3f;
	private bool onAtStart;

	private void Start()
	{
		widgetOneCanvasGroup.alpha = onAtStart ? 1f : 0f;
		widgetTwoCanvasGroup.alpha = onAtStart ? 0f : 1f;

		InvokeRepeating("Switch", timeBetweenRotation, timeBetweenRotation);
	}
	
	private void Switch()
	{
		StartCoroutine(SwitchRoutine());
	}

	private IEnumerator SwitchRoutine()
	{
		StartCoroutine(Fade(widgetOneCanvasGroup, onAtStart));
		StartCoroutine(Fade(widgetTwoCanvasGroup, !onAtStart));
		onAtStart = !onAtStart;

		yield return null;
	}

	private IEnumerator Fade(CanvasGroup widget, bool fadeOut)
	{
		widget.alpha = fadeOut ? 1f : 0f;

		if (fadeOut)
		{
			print(widget.name + " fading out");

			while (widget.alpha > 0f)
			{
				widget.alpha -= Time.deltaTime * fadeSpeed;
				yield return null;
			}
		}
		else
		{
			while (widget.alpha < 1f)
			{
				widget.alpha += Time.deltaTime * fadeSpeed;
				yield return null;
			}
		}
	}
}
