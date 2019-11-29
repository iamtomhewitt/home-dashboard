using System.Collections;
using UnityEngine;

public abstract class FadingWidget : Widget
{
	[SerializeField] private AnimationClip fadeOutAnimation;
	[SerializeField] private AnimationClip fadeInAnimation;

	private Animator animator;

	public void SetAnimator(Animator animator)
	{
		this.animator = animator;
	}

	public void FadeIn()
	{
		animator.Play(fadeInAnimation.name);
	}

	public void FadeOut()
	{
		animator.Play(fadeOutAnimation.name);
	}

	public float GetFadeInLength()
	{
		return fadeInAnimation.length;
	}

	public float GetFadeOutLength()
	{
		return fadeOutAnimation.length;
	}

	public IEnumerator Fade(MethodToCallBetweenFading method)
	{
		FadeOut();

		yield return new WaitForSeconds(GetFadeOutLength());

		method();

		yield return new WaitForSeconds(0.15f);

		FadeIn();
	}
}

public delegate void MethodToCallBetweenFading();
