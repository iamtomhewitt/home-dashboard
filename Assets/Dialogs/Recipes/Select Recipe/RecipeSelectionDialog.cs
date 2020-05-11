using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;
using Requests;
using Planner;
using TMPro;

namespace Dialog
{
    /// <summary>
    /// A dialog that shows all the available recipes to choose from when selecting a recipe for a planner entry on the food planner.
    /// </summary>
    public class RecipeSelectionDialog : PopupDialog
    {
        [Header("Recipe Selection Dialog Settings")]
        [SerializeField] private RecipeEntry recipeEntryPrefab;
        [SerializeField] private Transform scrollViewContent;
        [SerializeField] private Button freeTextButton;
        [SerializeField] private Button addRecipeButton;
        [SerializeField] private Image freeTextInput;
        [SerializeField] private Image scrollBackground;
        [SerializeField] private Image scrollHandle;
        [SerializeField] private TMP_Text loadingText;

        private string selectedRecipe;
        private string freeTextRecipe;

        /// <summary>
        /// Populates the scroll view with recipes retrieved from the recipe manager on Heroku.
        /// </summary>
        public void PopulateRecipes()
        {
            StartCoroutine(PopulateRecipesRoutine());
        }

        private IEnumerator PopulateRecipesRoutine()
        {
            ClearExistingRecipes();

            loadingText.text = "Loading...";

            UnityWebRequest request = Postman.CreateGetRequest(Endpoints.instance.RECIPES(Config.instance.GetWidgetConfig()[FindObjectOfType<FoodPlanner>().GetWidgetConfigKey()]["apiKey"]));
            yield return request.SendWebRequest();
            string response = request.downloadHandler.text;

            JSONNode json = JSON.Parse(response);
            for (int i = 0; i < json["recipes"].AsArray.Count; i++)
            {
                RecipeEntry entry = Instantiate(recipeEntryPrefab.gameObject, scrollViewContent).GetComponent<RecipeEntry>();
                entry.SetRecipeText(json["recipes"][i]["name"]);
                entry.SetParentDialog(this);
                entry.SetSteps(json["recipes"][i]["steps"].AsArray);
                entry.SetIngredients(json["recipes"][i]["ingredients"].AsArray);
            }

            loadingText.text = "";
        }

        /// <summary>
        /// Stops duplicate recipes showing.
        /// </summary>
        private void ClearExistingRecipes()
        {
            foreach (Transform child in scrollViewContent)
            {
                Destroy(child.gameObject);
            }
        }

        public void SelectFreeTextRecipe(InputField freeTextInput)
        {
            freeTextRecipe = freeTextInput.text;
            selectedRecipe = "";
            freeTextInput.text = "";
            SetFinished();
            Hide();
        }

        public void SelectRecipe(string recipe)
        {
            freeTextRecipe = "";
            selectedRecipe = recipe;
            SetFinished();
            Hide();
        }

        public string GetFreeTextRecipeName()
        {
            return freeTextRecipe;
        }

        public string GetSelectedRecipe()
        {
            return selectedRecipe;
        }

        public void HideAndCancelResult()
        {
            Hide();
            SetCancel();
        }

        public override void ApplyAdditionalColours(Color mainColour, Color textColour)
        {
            freeTextButton.GetComponent<Image>().color = mainColour;
            freeTextButton.GetComponentInChildren<TMP_Text>().color = textColour;

            addRecipeButton.GetComponent<Image>().color = mainColour;
            addRecipeButton.GetComponentInChildren<TMP_Text>().color = textColour;

            freeTextInput.color = mainColour;

            scrollBackground.color = Colours.Darken(mainColour);
            scrollHandle.color = Colours.Lighten(mainColour);
        }

        public void ShowAddNewRecipeDialog()
        {
            FindObjectOfType<AddNewRecipeDialog>().Show();
        }
    }
}