using System;
using UnityEngine;
using UnityEngine.UI;

public class Clock : Widget
{
	[Space(15f)]
	public Text clockText;
	public Text dateText;

	private void Start()
	{
		this.Initialise();
		InvokeRepeating("Run", 0f, ToSeconds(this.timeUnit, this.repeatRate));
	}

	public override void Run()
	{
		clockText.text = DateTime.Now.ToString("HH:mm:ss");
		dateText.text = DateTime.Now.ToString("dd MMMM yyyy");
	}
}
