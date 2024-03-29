﻿using Dialog;
using Requests;
using SimpleJSON;
using System.Collections;
using TMPro;
using UnityEngine.Networking;
using UnityEngine;

namespace OnlineLists
{
	public class OnlineListEntry : MonoBehaviour
	{
		[SerializeField] private TMP_Text nameText;
		[SerializeField] private TMP_Text removeButtonText;
		[SerializeField] private string taskId;

		private string apiKey;

		public void SetNameText(string text)
		{
			nameText.text = text;
		}

		public void SetNameTextColour(Color colour)
		{
			nameText.color = colour;
		}

		public void SetTaskId(string id)
		{
			taskId = id;
		}

		public void SetApiKey(string apiKey)
		{
			this.apiKey = apiKey;
		}

		public void SetRemoveButtonTextColour(Color colour)
		{
			removeButtonText.color = colour;
		}

		/// <summary>
		/// Called from the 'X' button.
		/// </summary>
		public void Remove()
		{
			StartCoroutine(RemoveRoutine());
		}

		private IEnumerator RemoveRoutine()
		{
			ConfirmDialog dialog = FindObjectOfType<ConfirmDialog>();
			dialog.ApplyColours();
			dialog.Show();
			dialog.SetInfoMessage("Remove '<b>" + nameText.text + "</b>'?");
			dialog.SetNone();

			while (dialog.IsNone())
			{
				yield return null;
			}

			if (dialog.IsNo() || dialog.IsCancel())
			{
				dialog.Hide();
				yield break;
			}

			if (dialog.IsYes())
			{
				dialog.Hide();

				string url = string.Format("{0}/{1}/close", Endpoints.instance.TODOIST_TASKS(), taskId);
				UnityWebRequest request = Postman.CreatePostRequest(url, new JSONObject());
				request.SetRequestHeader("Authorization", "Bearer " + apiKey);
				yield return request.SendWebRequest();

				Destroy(this.gameObject);
			}
		}
	}
}