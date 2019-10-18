using UnityEngine;

public class FoodPlanner : Widget
{
	private void Start()
	{
		this.Initialise();
		InvokeRepeating("Run", 0f, ToSeconds());
	}

	public override void Run()
	{
		
	}
}
