﻿using OnlineLists;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace Dialog
{
	/// <summary>
	/// A dialog that adds an item to an online list.
	/// </summary>
	public class AddItemDialog : WidgetDialog
	{
		[Header("Add Item Settings")]
		[SerializeField] private Image addButtonColour;
		[SerializeField] private TMP_Text addButtonText;
		[SerializeField] private TMP_Text statusText;

		private OnlineList list;

		/// <summary>
		/// Called from a button.
		/// </summary>
		public void ResetStatusText()
		{
			statusText.text = "";
		}

		public void SetStatusText(string text)
		{
			statusText.text = text;
		}

		public void SetOnlineList(OnlineList list)
		{
			this.list = list;
		}

		/// <summary>
		/// Called from a button on the dialog.
		/// </summary>
		public void AddItem(InputField input)
		{
			list.AddItem(input.text);
			list.Refresh();
			input.text = "";
		}

		public override void ApplyAdditionalColours(Color mainColour, Color textColour)
		{
			addButtonColour.color = mainColour;
			addButtonText.color = textColour;
		}

		public override void PostShow()
		{
			ResetStatusText();
		}
	}
}