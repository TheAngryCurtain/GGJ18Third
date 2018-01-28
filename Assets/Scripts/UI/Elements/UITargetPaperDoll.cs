using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UITargetPaperDoll : MonoBehaviour
    {
        [SerializeField]
        private Image m_HatImage;

        [SerializeField]
        private Image m_ClothingImage;

        [SerializeField]
        private Image m_PantsImage;

        public void SetData(eClothingType type, Color color)
        {
            switch (type)
            {
                case eClothingType.Coat:
                    m_ClothingImage.color = color;
                    break;
                case eClothingType.Hat:
                    m_HatImage.color = color;
                    break;
                case eClothingType.Pants:
                    m_PantsImage.color = color;
                    break;
            }
        }
    }
}

