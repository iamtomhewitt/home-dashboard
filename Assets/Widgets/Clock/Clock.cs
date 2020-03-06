using UnityEngine;
using System;
using SimpleJSON;
using TMPro;

namespace Clock
{
	public class Clock : Widget
	{
		[Header("Clock Settings")]
		[SerializeField] private TMP_Text clockText;
		[SerializeField] private TMP_Text dateText;

		public override void Start()
		{
			base.Start(); 
			
			// Reload the config in an Invoke method as we don't want to reload config every second
			InvokeRepeating("ReloadConfig", 2f, 3600f);
		}

		public override void ReloadConfig() 
		{
			JSONNode config = Config.instance.GetWidgetConfig()[this.GetWidgetConfigKey()];
			clockText.color = GetTextColour();
			dateText.color = Colours.ToColour(config["dateColour"]);
		}

		public override void Run()
		{
			clockText.text = DateTime.Now.ToString("HH:mm:ss");
			dateText.text = DateTime.Now.ToString("dd MMMM yyyy");
		}
	}
}