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
            stepsText.text = "";

            for (int i = 0; i < steps.Count; i++)
            {
                stepsText.text += string.Format("{0}. {1} \n\n", (i+1), steps[i]);
            }
        }
    }
}