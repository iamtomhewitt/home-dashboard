using UnityEngine;
using UnityEngine.UI;
using OnlineLists;

namespace Dialog
{
	namespace OnlineLists
	{
		public class AddItemDialog : Dialog
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

			/// <summary>
			/// Called from a Unity button when the dialog is shown (in this case, when pressing Add Item on an OnlineList)
			/// </summary>
			public void ApplyColours(Widget widget)
			{
				SetTopBarColour(widget.GetWidgetColour());
				SetDialogTitleColour(widget.GetTextColour());
				SetHideButtonTextColour(widget.GetTextColour());
				SetHideButtonColour(widget.GetWidgetColour());
				addButtonColour.color = widget.GetWidgetColour();
				addButtonText.color = widget.GetTextColour();
			}
		}
	}
}
