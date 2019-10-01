using System;
using UnityEngine;
using UnityEngine.UI;

public class Clock : Widget
{
	[Space(15f)]
	[SerializeField] private Text clockText;

	private void Start()
	{
		this.Initialise();
		InvokeRepeating("Run", 0f, this.repeatRate);
	}

	public override void Run()
	{
		clockText.text = DateTime.Now.ToString("HH:mm:ss");
	}
}
