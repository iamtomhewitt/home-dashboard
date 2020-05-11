using UnityEngine;
using Dialog;
using TMPro;
using System.Collections.Generic;
using SimpleJSON;

namespace Planner
{
    /// <summary>
    /// The recipes retrieved from Heroku shown on the recipe selection dialog.
    /// </summary>
    public class RecipeEntry : MonoBehaviour
    {
        [SerializeField] private TMP_Text recipe;

        private List<string> steps = new List<string>();
        private RecipeSelectionDialog dialog;

        public void SetRecipeText(string message)
        {
            recipe.text = message;
        }

        public void SetSteps(JSONArray array)
        {
            foreach (JSONNode s in array)
            {
                steps.Add((string)s);
            }
        }

        public void SetParentDialog(RecipeSelectionDialog dialog)
        {
            this.dialog = dialog;
        }

        /// <summary>
        /// Called when a recipe is selected from the list of available recipes.
        /// </summary>
        public void Select()
        {
            dialog.SelectRecipe(recipe.text);
        }
        
        public void ShowSteps()
        {
            RecipeStepsDialog stepsDialog = FindObjectOfType<RecipeStepsDialog>();
            stepsDialog.Show();
            stepsDialog.SetDialogTitleText(recipe.text + " Steps");
            stepsDialog.SetStepsText(steps);
        }
    }
}