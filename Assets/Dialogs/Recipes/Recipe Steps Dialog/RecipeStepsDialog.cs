using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dialog
{
    public class RecipeStepsDialog : PopupDialog
    {
        [SerializeField] private TMP_Text ingredientsText;
        [SerializeField] private TMP_Text noIngredientsText;
        [SerializeField] private TMP_Text noStepsText;
        [SerializeField] private TMP_Text stepsText;

        public override void ApplyAdditionalColours(Color mainColour, Color textColour)
        {
            // Nothing to do
        }

		public override void PostShow()
		{
			// Nothing to do
		}

        public void SetStepsText(List<string> steps)
        {
            if (steps.Count == 0)
            {
                stepsText.text = "";
                noStepsText.text = "No steps available for this recipe!";
            }
            else
            {
                string newLine = "\n\n";
                stepsText.text = "";
                noStepsText.text = "";

                for (int i = 0; i < steps.Count; i++)
                {
                    stepsText.text += string.Format("{0}. {1} {2}", (i + 1), steps[i], newLine);
                }

                // Remove last new line
                stepsText.text = stepsText.text.Substring(0, stepsText.text.Length - newLine.Length);
            }
        }

        public void SetIngredientsText(List<string> ingredients)
        {
            if (ingredients.Count == 0)
            {
                ingredientsText.text = "";
                noIngredientsText.text = "No ingredients in this recipe!";
            }
            else
            {
                string newLine = "\n\n";
                ingredientsText.text = "";
                noIngredientsText.text = "";

                for (int i = 0; i < ingredients.Count; i++)
                {
                    ingredientsText.text += string.Format("• {0} {1}", ingredients[i], newLine);
                }

                // Remove last new line
                ingredientsText.text = ingredientsText.text.Substring(0, ingredientsText.text.Length - newLine.Length);
            }
        }
    }
}