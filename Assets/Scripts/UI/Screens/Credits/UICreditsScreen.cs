using Rewired;
using System.Collections;
using System.Collections.Generic;
using UI.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UICreditsScreen : UIBaseScreen
    {
        public override void SetPrompts()
        {
            List<InputButton> leftPrompts = new List<InputButton>();
            leftPrompts.Add(InputButton.B);

            UIManager.Instance.SetLeftPrompts(leftPrompts);

            base.SetPrompts();
        }

        protected override void OnInputUpdate(InputActionEventData data)
        {
            if (data.actionId == RewiredConsts.Action.UICancel)
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


