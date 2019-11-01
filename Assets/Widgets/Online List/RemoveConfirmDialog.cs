using UnityEngine;
using UnityEngine.UI;

namespace OnlineLists
{
    public class RemoveConfirmDialog : Dialog
    {
        [SerializeField] private Text infoText;
        private DialogResult result;

        private void Start()
        {
            Hide();
        }

        /// <summary>
        /// Has to be a reposition otherwise other components cannot find this if the Gameobject is deactivated
        /// </summary>
        public override void Hide()
        {
            GetComponent<RectTransform>().localPosition = new Vector2(-100f, transform.position.y);
        }

        /// <summary>
        /// Has to be a reposition otherwise other components cannot find this if the Gameobject is deactivated
        /// </summary>
        public override void Show()
        {
            GetComponent<RectTransform>().localPosition = Vector2.zero;
        }

        public void SetInfoMessage(string message)
        {
            infoText.text = message;
        }

        public void SetNone()
        {
            result = DialogResult.NONE;
        }

        public DialogResult GetResult()
        {
            return result;
        }

        public void Yes()
        {
            this.result = DialogResult.YES;
        }

        public void No()
        {
            this.result = DialogResult.NO;
        }

        public void Cancel()
        {
            this.result = DialogResult.CANCEL;
        }
    }
}
