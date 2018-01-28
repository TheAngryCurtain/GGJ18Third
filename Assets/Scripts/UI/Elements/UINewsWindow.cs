using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UINewsWindow : MonoBehaviour
    {
        [SerializeField]
        private UINewsEntry m_NewsEntryPrefab;

        [SerializeField]
        private VerticalLayoutGroup m_NewsWindowLG;

        private FixedLengthLinkedList<UINewsEntry> m_NewsEntryList;

        void Awake()
        {
            m_NewsEntryList = new FixedLengthLinkedList<UINewsEntry>(3);
        }

        private UINewsEntry InstantiateScreenEntry()
        {
            UINewsEntry newScreenEntry = GameObject.Instantiate<UINewsEntry>(m_NewsEntryPrefab, m_NewsWindowLG.transform);

            Debug.Assert(newScreenEntry != null, "UINewsEntry.InstantiateScreenEntry - Failed to instantiate UINewsEntry, ensure that the prefab is valid and that the layout group is not null.");

            return newScreenEntry;
        }

        public void AddNewNewsEntry(string entry)
        {
            UINewsEntry popedEntry = m_NewsEntryList.Push_Front(InstantiateScreenEntry());

            m_NewsEntryList.Front().SetData(entry);

            if (popedEntry != null)
            {
                GameObject.Destroy(popedEntry.gameObject);
            }
        }

        private void OnDestroy()
        {
        }
    }
}