using UnityEngine;
using UnityEngine.UI;
using OnlineLists;

namespace Dialog
{
	namespace OnlineLists
	{
		public class AddItemDialog : Dialog
		{
			[SerializeField] private OnlineList list;
			[SerializeField] private Text statusText;

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
		}
	}
}
