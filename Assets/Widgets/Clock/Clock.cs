using System;
using UnityEngine;
using UnityEngine.UI;

namespace Clock
{
	public class Clock : Widget
	{
		[Header("Clock Settings")]
		[SerializeField] private Text clockText;
		[SerializeField] private Text dateText;

		private void Start()
		{
			this.Initialise();
			InvokeRepeating("Run", 0f, RepeatRateInSeconds());
		}

		public override void Run()
		{
			clockText.text = DateTime.Now.ToString("HH:mm:ss");
			dateText.text = DateTime.Now.ToString("dd MMMM yyyy");
		}
	}
}