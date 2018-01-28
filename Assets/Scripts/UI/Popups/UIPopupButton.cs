using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Enums;

namespace UI
{
    public class UIPopupButton : UIButton
    {
        protected PopupSelection m_Selection = PopupSelection.Okay;
        public PopupSelection Selection { get { return m_Selection; } }

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
            if(data is UIPopupButtonData)
            {
                UIPopupButtonData popupData = (UIPopupButtonData)data;

                m_Selection = popupData.Selection;

                m_Text.text = popupData.Text;
            }

            base.SetData(data);
        }
    }
}