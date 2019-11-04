﻿using UnityEngine;
using UnityEngine.UI;

namespace OnlineLists
{
    public class ConfirmDialog : Dialog
    {
        [SerializeField] private Text infoText;

        private void Start()
        {
            Hide();
        }

        public void SetInfoMessage(string message)
        {
            infoText.text = message;
        }

        public void SetNone()
        {
            SetResult(DialogResult.NONE);
        }

        public void Yes()
        {
			SetResult(DialogResult.YES);
        }

        public void No()
        {
			SetResult(DialogResult.NO);
        }

        public void Cancel()
        {
			SetResult(DialogResult.CANCEL);
        }
    }
}
