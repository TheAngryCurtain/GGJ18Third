namespace UI
{
    namespace Enums
    {
        public enum InputPlatform
        {
            Keyboard,
            Joystick
        }

        public enum InputButton
        {
            None,
            A,
            B,
            X,
            Y,
            BACK,
            START,
            RB,
            LB,
            RT,
            LT,
            LStick,
            LStick_Left,
            LStick_Right,
            LStick_Up,
            LStick_Down,
            LStick_Press,
            RStick,
            RStick_Left,
            RStick_Right,
            RStick_Up,
            RStick_Down,
            RStick_Press,
            DPad,
            DPad_Left,
            DPad_Right,
            DPad_Up,
            DPad_Down,

            // Keyboard + Mouse
            Left_Click,
            Right_Click,
            Enter,
            Space_Bar,
            Escape,
            Tab
        }

        public enum ScreenId
        {
            None = -1,
            MainMenu,
            Splash,
            Title,
            Options,
            Credits,
            HUD,
            Letter
        }

        public enum UIScreenAnimState
        {
            None = -1,
            Intro,
            Outro
        }

        public enum UIScreenAnimEvent
        {
            None = -1,
            Start,
            End
        }

        public enum UINavigationType
        {
            None = -1,
            Advance,
            Back
        }

        public enum PopupOptions
        {
            Okay = 0,
            Cancel,
            OkayCancel,
        }

        public enum PopupSelection
        {
            Okay = 0,
            Cancel
        }
    }
}
