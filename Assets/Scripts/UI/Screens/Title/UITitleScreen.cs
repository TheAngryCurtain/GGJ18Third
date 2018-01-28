using System.Collections;
using System.Collections.Generic;
using Rewired;
using UI.Enums;
using UnityEngine;

namespace UI
{
    public class UITitleScreen : UIBaseScreen
    {
        public override void Start()
        {
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
            //List<InputButton> leftPrompts = new List<InputButton>();
            //leftPrompts.Add(InputButton.A);

            //UIManager.Instance.SetLeftPrompts(leftPrompts);

            base.SetPrompts();
        }

        public override void Shutdown()
        {
            UIManager.Instance.ClearPrompts();

            base.Shutdown();
        }

        protected override void OnInputUpdate(InputActionEventData data)
        {
            if (data.actionId == RewiredConsts.Action.UISelect)
            {
                if (data.GetButtonUp())
                {
                    UIManager.Instance.TransitionToScreen(ScreenId.MainMenu);
                }
            }
            base.OnInputUpdate(data);
        }
    }
}
