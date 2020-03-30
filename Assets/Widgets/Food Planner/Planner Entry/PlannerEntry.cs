using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using Dialog;
using SimpleJSON;
using Requests;
using TMPro;

namespace Planner
{
    public class PlannerEntry : MonoBehaviour
    {
        [SerializeField] private Day day;
        [SerializeField] private TMP_Text dayText;
        [SerializeField] private TMP_Text recipe;
        [SerializeField] private Image recipeBackground;
        [SerializeField] private Image dayBackground;

        private JSONNode config;
        private string configKey;
        private string apiKey;
        private string plannerId;

        private IEnumerator Start()
        {
            configKey = FindObjectOfType<FoodPlanner>().GetWidgetConfigKey();
            config = Config.instance.GetWidgetConfig();
            apiKey = config[configKey]["apiKey"];
            plannerId = config[configKey]["plannerId"];

            string label = "";
            foreach (char c in day.ToString().Substring(0, 3).ToUpper())
            {
                label += c + "\n";
            }

            dayText.text = label;
			recipe.text	= "Loading...";

            UnityWebRequest request = Postman.CreateGetRequest(Endpoints.instance.PLANNER(day.ToString(), plannerId, apiKey));
            yield return request.SendWebRequest();

            bool ok = request.error == null ? true : false;
            if (!ok)
            {
                WidgetLogger.instance.Log("Error: " + (string)JSON.Parse(request.downloadHandler.text)["message"]);
                yield break;
            }

            recipe.text = (request.responseCode == 503) ? "Service Unavailable" : (string)JSON.Parse(request.downloadHandler.text)["planner"]["recipe"];
        }

        /// <summary>
        /// Called from a Unity button when the planner entry is clicked on.
        /// </summary>
        public void SelectRecipe()
        {
            StartCoroutine(SelectRecipeRoutine());
        }

        public IEnumerator SelectRecipeRoutine()
        {
            RecipeSelectionDialog dialog = FindObjectOfType<RecipeSelectionDialog>();
            dialog.Show();
            dialog.SetNone();
            dialog.PopulateRecipes();

            while (!dialog.IsFinished() && !dialog.IsCancel())
            {
                yield return null;
            }

            if (dialog.IsFinished())
            {
                recipe.text = !string.IsNullOrEmpty(dialog.GetSelectedRecipe()) ? dialog.GetSelectedRecipe() : dialog.GetFreeTextRecipeName();

                // Now update the planner online
                JSONObject body = JsonBody.AddToPlanner(string.IsNullOrEmpty(recipe.text) ? " " : recipe.text, day.ToString(), apiKey, plannerId);
                UnityWebRequest request = Postman.CreatePostRequest(Endpoints.instance.PLANNER_ADD(), body);
                yield return request.SendWebRequest();

                yield break;
            }
        }

        /// <summary>
        /// Quick method to clear the entry
        /// </summary>
        public void ClearRecipe()
        {
            StartCoroutine(ClearRecipeRoutine());
        }

        private IEnumerator ClearRecipeRoutine()
        {
            JSONObject body = JsonBody.AddToPlanner(" ", day.ToString(), apiKey, plannerId);
            UnityWebRequest request = Postman.CreatePostRequest(Endpoints.instance.PLANNER_ADD(), body);
            yield return request.SendWebRequest();

            JSONNode response = JSON.Parse(request.downloadHandler.text);
            if (response["status"] == 200)
            {
                recipe.text = "";
            }
            else
            {
                WidgetLogger.instance.Log("Could not clear recipe: " + response["message"]);
            }
        }

        public string GetRecipeName()
        {
            return recipe.text;
        }

        public void SetRecipeTextColour(Color colour)
        {
            recipe.color = colour;
        }

        public void SetDayTextColour(Color colour)
        {
            dayText.color = colour;
        }

        public void SetRecipeBackgroundColour(Color colour)
        {
            recipeBackground.color = colour;
        }

        public void SetDayBackgroundColour(Color colour)
        {
            dayBackground.color = colour;
        }

        private enum Day { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }
    }
}