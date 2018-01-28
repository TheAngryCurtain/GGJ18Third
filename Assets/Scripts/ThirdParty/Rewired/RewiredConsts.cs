/* Rewired Constants
   This list was generated on 1/28/2018 11:11:30 AM
   The list applies to only the Rewired Input Manager from which it was generated.
   If you use a different Rewired Input Manager, you will have to generate a new list.
   If you make changes to the exported items in the Rewired Input Manager, you will need to regenerate this list.
*/

namespace RewiredConsts {
    public static class Action {
        // Default
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Selection")]
        public const int SELECT = 0;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Move Up")]
        public const int ANALOG_UP = 2;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Move Down")]
        public const int ANALOG_DOWN = 3;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Move Left")]
        public const int ANALOG_LEFT = 4;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Move Right")]
        public const int ANALOG_RIGHT = 5;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Left Stick Horizonal")]
        public const int LEFTSTICK_HORIZONTAL = 8;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "LEFTSTICK_VERTICAL")]
        public const int LEFTSTICK_VERTICAL = 9;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Horizontal UI Control")]
        public const int UIHorizontal = 10;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Vertical UI Axis")]
        public const int UIVertical = 11;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Select UI Element")]
        public const int UISelect = 12;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Cancel UI")]
        public const int UICancel = 13;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "HatSwap")]
        public const int HatSwap = 14;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "BodySwap")]
        public const int BodySwap = 15;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "LegSwap")]
        public const int LegSwap = 16;
    }
    public static class Category {
        public const int Default = 0;
    }
    public static class Layout {
        public static class Joystick {
            public const int Default = 0;
            public const int UI = 1;
        }
        public static class Keyboard {
            public const int Default = 0;
            public const int UI = 1;
        }
        public static class Mouse {
            public const int Default = 0;
        }
        public static class CustomController {
            public const int Default = 0;
        }
    }
}
