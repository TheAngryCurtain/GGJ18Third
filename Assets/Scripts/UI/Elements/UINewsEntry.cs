using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UINewsEntry : MonoBehaviour
    {
        [SerializeField]
        private Text m_NewsText;

        public void SetData(string text)
        {
            m_NewsText.text = text;
        }
    }
}
