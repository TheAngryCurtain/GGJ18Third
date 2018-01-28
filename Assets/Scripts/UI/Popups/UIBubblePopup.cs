using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIBubblePopup : UIBasePopup
    {
        /// <summary>
        /// Set up any initial data or state information.
        /// </summary>
        public override void Initialize()
        {
            m_Buttons = new List<UIPopupButton>();

            if (mPopupData != null)
            {
                for (int i = 0; i < mPopupData.ButtonData.Count; ++i)
                {
                    UIBubblePopupButton button = GameObject.Instantiate<UIBubblePopupButton>(m_UIButtonPrefab.GetComponent<UIBubblePopupButton>(), Vector3.zero, Quaternion.identity, m_ButtonMount);
                    button.transform.localScale = Vector3.one;

                    button.SetData(mPopupData.ButtonData[i]);
                    button.Initialize();

                    button.AddOnClickDelegate(OnButtonClicked);

                    m_Buttons.Add(button);
                }

                onClickDelegate += mPopupData.Callback;

                m_PopupTitle.text = mPopupData.Title;
                m_PopupBody.text = mPopupData.Body;

                RectTransform myRect = GetComponent<RectTransform>();
                Vector2 myPositionOnScreen = new Vector2((mPopupData as UIBubblePopupData).BubblePos.x, (mPopupData as UIBubblePopupData).BubblePos.y);
                float scaleFactor = UIManager.Instance.PopupCanvas.scaleFactor;
                Vector2 finalPosition = new Vector2(myPositionOnScreen.x / scaleFactor, myPositionOnScreen.y / scaleFactor);
                myRect.anchoredPosition = finalPosition;
            }

            // Hook up our input listening.
            InputManager.Instance.AddInputEventDelegate(OnInputUpdate, UpdateLoopType.Update);
        }

        /// <summary>
        /// Sets the element data
        /// </summary>
        /// <param name="data">generic object data</param>
        public override void SetData(object data)
        {
            if (data is UIBubblePopupData)
            {
                mPopupData = (UIBubblePopupData)data;
            }

            mData = data;
        }

        /// <summary>
        /// Handles input from Rewired.
        /// </summary>
        /// <param name="data"></param>
        protected virtual void OnInputUpdate(InputActionEventData data)
        {
            if (data.actionId == RewiredConsts.Action.UISelect)
            {
                if (data.GetButtonUp())
                {
                    if(m_Buttons[0].Selection == Enums.PopupSelection.Okay)
                    {
                        m_Buttons[0].OnClick();
                    }
                }
            }
            else if (data.actionId == RewiredConsts.Action.UICancel)
            {
                if (data.GetButtonUp())
                {
                    if (m_Buttons[1].Selection == Enums.PopupSelection.Cancel)
                    {
                        m_Buttons[1].OnClick();
                    }
                }
            }
        }

        protected override void OnButtonClicked(object buttonData)
        {
            if (buttonData is UIBubbleButtonData)
            {
                UIBubbleButtonData data = (UIBubbleButtonData)buttonData;

                // If the onClickDelegate is null here, we have no popup interaction and that's pretty bad.
                Debug.Assert(onClickDelegate != null);

                if (onClickDelegate != null)
                {
                    onClickDelegate(data.Selection, mData);
                }
            }
        }

        public override void OnDestroy()
        {
            InputManager.Instance.RemoveInputEventDelegate(OnInputUpdate);
            base.OnDestroy();
        }
    }
}