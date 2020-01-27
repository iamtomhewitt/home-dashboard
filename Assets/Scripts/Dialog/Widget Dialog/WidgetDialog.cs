using UnityEngine;

namespace Dialog
{
	/// <summary>
	/// A dialog that is based upon a widget.
	/// </summary>
	public abstract class WidgetDialog : Dialog
	{
		[Header("Widget Dialog Settings")]
		[SerializeField] private Widget parentWidget;

		public void SetParentWidget(Widget widget)
		{
			parentWidget = widget;
		}

		public Widget GetParentWidget()
		{
			return parentWidget;
		}

		public override void ApplyColours()
		{
			Color mainColour = parentWidget.GetWidgetColour();
			Color textColour = parentWidget.GetTextColour();

			SetDialogTitleColour(textColour);
			SetTopBarColour(mainColour);
			SetHideButtonColour(mainColour, textColour);

			ApplyAdditionalColours(mainColour, textColour);
		}
	}
}