using System.Collections;
using System.Collections.Generic;
using UI.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIMainMenu : UIBaseScreen
    {
        /// MonoBehavior Implementation
        public override void Start()
        {
            for(int i = 0; i < m_IneractableScreenElements.Count; ++i)
            {
                UIMainMenuButtonData bData = new UIMainMenuButtonData();
                bData.MainMenuButton = (ButtonEnums.eMainMenuButtons)i;
                m_IneractableScreenElements[i].SetData(bData);
                m_IneractableScreenElements[i].Initialize();
                m_IneractableScreenElements[i].AddOnClickDelegate(OnMainMenuButtonClicked);
            }

            UIManager.Instance.EventSystem.SetSelectedGameObject(m_IneractableScreenElements[0].gameObject);

            base.Start();
        }

        public override void Update()
        {
            base.Update();
        }

        // UIBaseScreen Implementation
        public override void Initialize()
        {
            InputManager.Instance.SetInputType("UI");
            base.Initialize();
        }

        public override void SetPrompts()
        {
            List<InputButton> leftPrompts = new List<InputButton>();
            leftPrompts.Add(InputButton.A);

            UIManager.Instance.SetLeftPrompts(leftPrompts);

            base.SetPrompts();
        }

        public override void Shutdown()
        {
            for (int i = 0; i < m_IneractableScreenElements.Count; ++i)
            {
                m_IneractableScreenElements[i].RemoveOnClickDelegate(OnMainMenuButtonClicked);
            }

            UIManager.Instance.ClearPrompts();

            base.Shutdown();
        }

        // Class Implementation
        bool wasclicked = false;
        private void OnMainMenuButtonClicked(object data)
        {
            if(wasclicked)
            {
                return;
            }
            if(data is UIMainMenuButtonData)
            {
                UIMainMenuButtonData bData = (UIMainMenuButtonData)data;

                if(bData.MainMenuButton == ButtonEnums.eMainMenuButtons.Play)
                {
                    TargetManager.Instance.GenerateTarget();
                    UIManager.Instance.TransitionToScreen(ScreenId.Letter, new UILetterScreenData { BodyText = TargetManager.Instance.CurrentTarget.Description, SenderText = TargetManager.Instance.CurrentTarget.InformantName });
                    wasclicked = true;
                }
                else if (bData.MainMenuButton == ButtonEnums.eMainMenuButtons.Options)
                {
                    UIManager.Instance.TransitionToScreen(ScreenId.Options);
                    wasclicked = true;
                }
                else if (bData.MainMenuButton == ButtonEnums.eMainMenuButtons.Credits)
                {
                    UIManager.Instance.TransitionToScreen(ScreenId.Credits);
                    wasclicked = true;
                }
                else if (bData.MainMenuButton == ButtonEnums.eMainMenuButtons.Quit)
                {
                    wasclicked = true;
                    Application.Quit();
                }
            }
        }
    }

}