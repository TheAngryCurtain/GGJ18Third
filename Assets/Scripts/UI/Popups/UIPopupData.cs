using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UI.Enums;
using UnityEngine;

namespace UI
{
    public class UIPopupData
    {
        public List<UIPopupButtonData> ButtonData;
        public string Title;
        public string Body;
        public UIBasePopup.OnPopupOptionSelected Callback;
    }
}
