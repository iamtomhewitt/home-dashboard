using UnityEngine;

/// <summary>
/// A fading widget that uses the Animator for its fading.
/// </summary>
public abstract class AnimatorFadingWidget : FadingWidget
{
	[Space(15f)]
	[SerializeField] private Animator animator;
	[SerializeField] private AnimationClip fadeOutAnimation;
	[SerializeField] private AnimationClip fadeInAnimation;

	public float GetFadeOutAnimationLength()
	{
		return fadeOutAnimation.length;
	}

	public override void FadeIn()
	{
		animator.Play(fadeInAnimation.name);
	}

	public override void FadeOut()
	{
		animator.Play(fadeOutAnimation.name);
	}
}
