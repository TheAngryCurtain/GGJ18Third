using System.Collections;
using System.Collections.Generic;
using UI.Enums;
using UnityEngine;

namespace UI
{
    public class UIBubblePopupButton : UIPopupButton
    {
        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetData(object data)
        {
            if (data is UIBubbleButtonData)
            {
                UIBubbleButtonData popupData = (UIBubbleButtonData)data;

                m_Selection = popupData.Selection;

                m_Text.text = popupData.Text;

                m_Image.sprite = UIManager.Instance.Prompts.CurrentPromptsInfo.GetButtonInfo(popupData.Button).tex;
            }

            base.SetData(data);
        }
    }
}