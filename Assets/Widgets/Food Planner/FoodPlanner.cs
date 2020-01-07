using UnityEngine;
using System.Collections;
using Dialog;
using Dialog.OnlineLists;

namespace FoodPlannerWidget
{
	public class FoodPlanner : Widget
	{
		[Header("Food Planner Settings")]
		[SerializeField] private AddItemDialog shoppingList;

		private void Start()
		{
			this.Initialise();
			InvokeRepeating("Run", 0f, RepeatRateInSeconds());
		}

		public override void Run()
		{
			// Nothing to do!
		}

		/// <summary>
		/// Adds all the ingredients from all the recipes to the shopping list.
		/// </summary>
		public void AddToShoppingList()
		{
			StartCoroutine(AddToShoppingListRoutine());
		}

		private IEnumerator AddToShoppingListRoutine()
		{
			ConfirmDialog dialog = FindObjectOfType<ConfirmDialog>();
			dialog.Show();
			dialog.None();
			dialog.SetInfoMessage("Add all ingredients from each recipe to the shopping list?");

			while (dialog.GetResult() == DialogResult.NONE)
			{
				yield return null;
			}

			if (dialog.GetResult() == DialogResult.NO)
			{
				WidgetLogger.instance.Log(this, "Result of dialog was No");

				dialog.Hide();
				dialog.None();
				yield break;
			}

			if (dialog.GetResult() == DialogResult.YES)
			{
				WidgetLogger.instance.Log(this, "Result of dialog was Yes");

				dialog.Hide();
			}
		}
	}
}