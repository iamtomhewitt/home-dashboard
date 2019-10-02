using System;
using UnityEngine;
using UnityEngine.UI;

public class Clock : Widget
{
	[Space(15f)]
	public Text clockText;
	public Text dateText;

	private int oneDay = 86400;

	private void Start()
	{
		this.Initialise();
		InvokeRepeating("Run", 0f, this.repeatRate);
		InvokeRepeating("UpdateDateText", 0f, oneDay);
	}

	public override void Run()
	{
		clockText.text = DateTime.Now.ToString("HH:mm:ss");
	}

	private void UpdateDateText()
	{
		dateText.text = DateTime.Now.ToString("dd MMMM yyyy");
	}
}
