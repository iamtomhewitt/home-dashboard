using UnityEngine;
using System.Collections;

public abstract class FadingWidget : Widget
{
	[SerializeField] private AnimationClip fadeOutAnimation;
	[SerializeField] private AnimationClip fadeInAnimation;

	private Animator animator;

	private float fadeInDelay = 0.15f;

	public void SetAnimator(Animator animator)
	{
		this.animator = animator;
	}

	public IEnumerator Fade(MethodToCallBetweenFading Method)
	{
		animator.Play(fadeOutAnimation.name);

		yield return new WaitForSeconds(fadeOutAnimation.length);

		Method();

		yield return new WaitForSeconds(fadeInDelay);

		animator.Play(fadeInAnimation.name);
	}
}

public delegate void MethodToCallBetweenFading();
