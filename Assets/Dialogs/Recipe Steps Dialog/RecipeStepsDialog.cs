using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dialog
{
    public class RecipeStepsDialog : PopupDialog
    {
        [SerializeField] private TMP_Text stepsText;

        public override void ApplyAdditionalColours(Color mainColour, Color textColour)
        {
            // Nothing to do
        }

        public void SetStepsText(List<string> steps)
        {
            string newLine = "\n\n";
            stepsText.text = "";

            for (int i = 0; i < steps.Count; i++)
            {
                stepsText.text += string.Format("{0}. {1} {2}", (i+1), steps[i], newLine);
            }

            // Remove last new line
            stepsText.text = stepsText.text.Substring(0, stepsText.text.Length - newLine.Length);
        }
    }
}