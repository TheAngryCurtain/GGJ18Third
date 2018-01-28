using System.Collections;
using System.Collections.Generic;
using Rewired;
using UI.Enums;
using UnityEngine;

namespace UI
{
    public class UIHUD : UIBaseScreen
    {
        [SerializeField]
        private UISuspicionMeter m_SuspicionMeter;

        [SerializeField]
        private UITargetPaperDoll m_PaperDoll;

        [SerializeField]
        private UINewsWindow m_NewsWindow;

        public override void Start()
        {
            EventManager.Instance.AddEventListener<YEO_SUS_CHANGED_EVENT>(OnSuspicionChanged);
            EventManager.Instance.AddEventListener<YEO_TALK_EVENT>(OnTalkInteraction);
           
            EventManager.Instance.AddEventListener<TargetUpdatedEvent>(OnTargetChanged);

            TargetHintInfo hint = TargetManager.Instance.GetNextHint();

            TargetUpdatedEvent targetUpdatedEvent = new TargetUpdatedEvent();
            targetUpdatedEvent.ColorInfo = hint.HintColorInfo;

            EventManager.Instance.FireEvent(targetUpdatedEvent);

            base.Start();
        }

        public override void Update()
        {
            base.Update();
        }

        // UIBaseScreen Implementation
        public override void Initialize()
        {
            InputManager.Instance.SetInputType("Default");
            base.Initialize();
        }

        public override void SetupComplete()
        {
            Game.Instance.StartGame();
            base.SetupComplete();
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

            EventManager.Instance.RemoveEventListener<YEO_SUS_CHANGED_EVENT>(OnSuspicionChanged);
            EventManager.Instance.RemoveEventListener<TargetUpdatedEvent>(OnTargetChanged);

            base.Shutdown();
        }

        protected override void OnInputUpdate(InputActionEventData data)
        {
            if (data.actionId == RewiredConsts.Action.UISelect)
            {
                //if (data.GetButtonUp())
                //{
                //    UIManager.Instance.TransitionToScreen(ScreenId.MainMenu);
                //}
            }
            base.OnInputUpdate(data);
        }

        public void OnSuspicionChanged(YEO_SUS_CHANGED_EVENT eventData)
        {
            m_SuspicionMeter.SetData(eventData.Suspicion);
            m_NewsWindow.AddNewNewsEntry("Your actions have caused your suspicion to rise... your current suspicion is: " + eventData.Suspicion);
        }

        public void OnTalkInteraction(YEO_TALK_EVENT eventData)
        {
            m_NewsWindow.AddNewNewsEntry("You talked to someone, they said; " + eventData.message + '"');
        }

        public void OnTargetChanged(TargetUpdatedEvent eventData)
        {
            m_PaperDoll.SetData(eventData.ColorInfo.ClothingType, eventData.ColorInfo.Color);
            m_NewsWindow.AddNewNewsEntry("You found out that the target's " + eventData.ColorInfo.ClothingType + " is: " + eventData.ColorInfo.Color);
        }
    }
}
