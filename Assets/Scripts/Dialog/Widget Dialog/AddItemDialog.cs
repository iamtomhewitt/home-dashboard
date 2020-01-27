using UnityEngine;
using UnityEngine.UI;
using OnlineLists;

namespace Dialog
{
	/// <summary>
	/// A dialog that adds an item to an online list.
	/// </summary>
	public class AddItemDialog : WidgetDialog
	{
		[Header("Add Item Settings")]
		[SerializeField] private Image addButtonColour;
		[SerializeField] private Text addButtonText;
		[SerializeField] private Text statusText;
		[SerializeField] private OnlineList list;

		/// <summary>
		/// Called from a button.
		/// </summary>
		public void ResetStatusText()
		{
			statusText.text = "";
		}

		/// <summary>
		/// Called from a button on the dialog.
		/// </summary>
		public void AddItem(InputField input)
		{
			list.AddItem(input.text);
			input.text = "";
		}

		public override void ApplyAdditionalColours(Color mainColour, Color textColour)
		{
			addButtonColour.color = mainColour;
			addButtonText.color = textColour;
		}
	}
}