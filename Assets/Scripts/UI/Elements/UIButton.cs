using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIButton : UIBaseElement
    {
        ///////////////////////////////////////////////////////////////////
        /// Constants
        ///////////////////////////////////////////////////////////////////
        public static readonly string Identifier = "UI.UIButton";

        ///////////////////////////////////////////////////////////////////
        /// Serialized Memeber Variables
        ///////////////////////////////////////////////////////////////////
        [SerializeField]
        protected Text m_Text;                          //! Serialized reference to the UnityEngine.UI.Text object.

        [SerializeField]
        protected Image m_Image;                        //! Serialized reference to the UnityEngine.UI.Image object.

        ///////////////////////////////////////////////////////////////////
        /// Private Memeber Variables
        ///////////////////////////////////////////////////////////////////

        protected Button m_Button;                    //! Reference to the UnityEngine.UI Button component on this class.

        ///////////////////////////////////////////////////////////////////
        /// MonoBehaviour Implementation
        ///////////////////////////////////////////////////////////////////

        public override void Awake()
        {
            // Set any Components. Should I make this a class function? Awake already sort of does it. Not really needed.
            m_Button = GetComponent<Button>();

            // Do our Validation Assert.
            Debug.AssertFormat(Validate() != false, "{0} : Failed to validate, please ensure that all required components are set and not null.", UIButton.Identifier);

            // Hack for now

            Initialize();

            base.Awake();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            base.Update();
        }

        ///////////////////////////////////////////////////////////////////
        /// UIBaseElement Implementation
        ///////////////////////////////////////////////////////////////////

        /// <summary>
        /// Sets the element data
        /// </summary>
        /// <param name="data">generic object data</param>
        public override void SetData(object data)
        {
            base.SetData(data);
        }

        /// <summary>
        /// Set up any initial data or state information.
        /// </summary>
        public override void Initialize()
        {
            // Isn't reflection great?
            if(m_Button != null)
            {
                m_Button.onClick.AddListener(OnClick);
            }

            base.Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool Validate()
        {
            bool isValid = false;

            isValid = (m_Button != null);

            return isValid && base.Validate();
        }

        /// <summary>
        /// Destory anything that keeps memory, remove any callbacks, whatever.
        /// </summary>
        public override void Shutdown()
        {
            base.Shutdown();
        }

        /// <summary>
        /// Callback to use with Unity's OnClick Event
        /// </summary>
        public override void OnClick()
        {
            base.OnClick();
        }

        /// <summary>
        /// Called when this element is destroyed
        /// </summary>
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}