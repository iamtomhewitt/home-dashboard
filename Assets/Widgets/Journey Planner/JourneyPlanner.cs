using UnityEngine;
using SimpleJSON;

public class JourneyPlanner : Widget
{
	private JSONNode config;

	private string startPoint;
	private string endPoint;
	private string apiKey;

	public override void ReloadConfig()
	{
		config = Config.instance.GetWidgetConfig()[this.GetWidgetConfigKey()];
		startPoint = config["startPoint"];
		endPoint = config["endPoint"];
		apiKey = config["apiKey"];
	}

	public override void Run()
	{
		this.ReloadConfig();
	}
}
