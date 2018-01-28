using System.Collections;
using System.Collections.Generic;
using Rewired;
using UI.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UILetterScreen : UIBaseScreen
    {
        [SerializeField]
        private string m_LetterString;

        [SerializeField]
        private string m_SignedString;

        [SerializeField]
        private string m_SignedByString;

        [SerializeField]
        private float m_CharDelay = 0.3f;

        [SerializeField]
        private Text m_BodyText;

        [SerializeField]
        private Text m_SignedText;

        [SerializeField]
        private Text m_SignedByText;

        private bool m_InputBlocked = true;

        public override void Start()
        {
            base.Start();
        }

        private IEnumerator WriteStrings()
        {
            int charCount = m_LetterString.Length;

            for (int i = 0; i < charCount; ++i)
            {
                yield return new WaitForEndOfFrame();
                m_BodyText.text += m_LetterString[i];

                yield return new WaitForSeconds(m_CharDelay);
            }

            yield return new WaitForSeconds(1.0f);

            charCount = m_SignedString.Length;

            for (int i = 0; i < charCount; ++i)
            {
                yield return new WaitForEndOfFrame();
                m_SignedText.text += m_SignedString[i];

                yield return new WaitForSeconds(m_CharDelay);
            }

            yield return new WaitForSeconds(0.5f);

            charCount = m_SignedByString.Length;

            for (int i = 0; i < charCount; ++i)
            {
                yield return new WaitForEndOfFrame();
                m_SignedByText.text += m_SignedByString[i];

                yield return new WaitForSeconds(m_CharDelay);
            }

            m_InputBlocked = false;

            List<InputButton> leftPrompts = new List<InputButton>();
            leftPrompts.Add(InputButton.A);

            UIManager.Instance.SetLeftPrompts(leftPrompts);
        }

        public override void Update()
        {
            base.Update();
        }

        // UIBaseScreen Implementation
        public override void Initialize()
        {
            InputManager.Instance.SetInputType("UI");
            m_InputBlocked = true;
            base.Initialize();
        }

        public override void SetPrompts()
        {
            base.SetPrompts();
        }

        public override void SetData(object data)
        {
            if(data is UILetterScreenData)
            {
                UILetterScreenData lData = data as UILetterScreenData;
                m_LetterString = lData.BodyText;
                m_SignedByString = lData.SenderText;
            }
            base.SetData(data);
        }

        public override void SetupComplete()
        {
            // Finished Intro Anim, do shit.
            StartCoroutine(WriteStrings());
            base.SetupComplete();
        }

        public override void Shutdown()
        {
            UIManager.Instance.ClearPrompts();

            base.Shutdown();
        }

        protected override void OnInputUpdate(InputActionEventData data)
        {
            if (!m_InputBlocked && data.actionId == RewiredConsts.Action.UISelect)
            {
                if (data.GetButtonUp())
                {
                    Game.Instance.LoadGameState(eGameState.GamePlay);
                    UIManager.Instance.TransitionToScreen(ScreenId.HUD);
                }
            }
            base.OnInputUpdate(data);
        }
    }
}
