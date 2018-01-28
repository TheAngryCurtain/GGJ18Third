using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UISuspicionMeter : MonoBehaviour
    {
        [SerializeField]
        private Image m_SuspicionBar;

        private RectTransform m_RectTransform;
        private float m_MaxWidth = 0.0f;

        private void Awake()
        {
            m_RectTransform = m_SuspicionBar.GetComponent<RectTransform>();
            m_MaxWidth = m_RectTransform.sizeDelta.x;
        }

        public void SetData(int suspicion)
        {
            float newX = (suspicion / 100.0f) * m_MaxWidth;
            m_RectTransform.sizeDelta = new Vector2(newX, m_RectTransform.sizeDelta.y);
        }
    }
}
