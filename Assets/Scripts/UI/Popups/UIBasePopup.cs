using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI.Enums;
using System;

namespace UI
{
    public class UIBasePopup : MonoBehaviour
    {
        ///////////////////////////////////////////////////////////////////
        /// Interaction Delegate
        ///////////////////////////////////////////////////////////////////

        public delegate void OnPopupOptionSelected(PopupSelection selection, object data);
        protected OnPopupOptionSelected onClickDelegate;

        ///////////////////////////////////////////////////////////////////
        /// Private Member Variables
        ///////////////////////////////////////////////////////////////////

        protected object mData = null;
        protected UIPopupData mPopupData = null;

        ///////////////////////////////////////////////////////////////////
        /// Serialized Member Variables
        ///////////////////////////////////////////////////////////////////

        [SerializeField]
        protected Text m_PopupTitle;

        [SerializeField]
        protected Text m_PopupBody;

        [SerializeField]
        protected UIPopupButton m_UIButtonPrefab;

        [SerializeField]
        protected Transform m_ButtonMount;

        ///////////////////////////////////////////////////////////////////
        /// Private Member Variables
        ///////////////////////////////////////////////////////////////////

        protected List<UIPopupButton> m_Buttons;

        ///////////////////////////////////////////////////////////////////
        /// MonoBehaviour Implementation
        ///////////////////////////////////////////////////////////////////

        public virtual void Awake()
        {
        }

        public virtual void Start()
        {
        }

        public virtual void Update()
        {
        }

        ///////////////////////////////////////////////////////////////////
        /// UIBaseElement Implementation
        ///////////////////////////////////////////////////////////////////

        /// <summary>
        /// Set up any initial data or state information.
        /// </summary>
        public virtual void Initialize()
        {
            m_Buttons = new List<UIPopupButton>();

            if (mPopupData != null)
            {
                for(int i = 0; i < mPopupData.ButtonData.Count; ++i)
                {
                    UIPopupButton button = GameObject.Instantiate<UIPopupButton>(m_UIButtonPrefab, Vector3.zero, Quaternion.identity, m_ButtonMount);
                    button.transform.localScale = Vector3.one;

                    button.SetData(mPopupData.ButtonData[i]);
                    button.Initialize();

                    button.AddOnClickDelegate(OnButtonClicked);

                    m_Buttons.Add(button);
                }

                onClickDelegate += mPopupData.Callback;

                m_PopupTitle.text = mPopupData.Title;
                m_PopupBody.text = mPopupData.Body;
            }
        }

        /// <summary>
        /// Validate the element. Base class returns true if there are buttons. Since popups (of this kind) need buttons.
        /// </summary>
        /// <returns></returns>
        protected virtual bool Validate()
        {
            bool isValid = true;

            isValid = isValid && m_ButtonMount != null;

            isValid = isValid && m_UIButtonPrefab != null;

            isValid = isValid && m_PopupTitle != null;

            isValid = isValid && m_PopupBody != null;

            return isValid;
        }

        /// <summary>
        /// Destory anything that keeps memory, remove any callbacks, whatever.
        /// </summary>
        public virtual void Shutdown()
        {
            for (int i = 0; i < m_Buttons.Count; ++i)
            {
                m_Buttons[i].RemoveOnClickDelegate(OnButtonClicked);
                m_Buttons[i].Shutdown();
                GameObject.Destroy(m_Buttons[i].gameObject);
            }

            m_Buttons = null;
        }

        /// <summary>
        /// Sets the element data
        /// </summary>
        /// <param name="data">generic object data</param>
        public virtual void SetData(object data)
        {
            if (data is UIPopupData)
            {
                mPopupData = (UIPopupData)data;
            }

            mData = data;
        }

        protected virtual void OnButtonClicked(object buttonData)
        {
            if(buttonData is UIPopupButtonData)
            {
                UIPopupButtonData data = (UIPopupButtonData)buttonData;

                // If the onClickDelegate is null here, we have no popup interaction and that's pretty bad.
                Debug.Assert(onClickDelegate != null);

                if(onClickDelegate != null)
                {
                    onClickDelegate(data.Selection, mData);
                }
            }
        }

        /// <summary>
        /// Called when this element is destroyed
        /// </summary>
        public virtual void OnDestroy()
        {
            if (onClickDelegate != null)
            {
                Delegate[] invocationList = onClickDelegate.GetInvocationList();

                for (int i = 0; i < invocationList.Length; ++i)
                {
                    invocationList[i] = null;
                }
            }
            onClickDelegate = null;
        }
    }
}