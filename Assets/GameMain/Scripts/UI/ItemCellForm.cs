using GameFramework;
using UnityEngine;
using UnityEngine.UI;

namespace KSG
{
    public class ItemCellForm : UGuiForm
    {
        [SerializeField]
        private Image m_Icon = null;

        [SerializeField]
        private Text m_CountText = null;

        [SerializeField]
        private Text m_NameText = null;

        private int m_ItemId = 0;

        public int ItemId
        {
            get
            {
                return m_ItemId;
            }
        }

        private void Awake()
        {
            CacheControls();
        }

        public void Refresh(InventoryItemStack stack)
        {
            CacheControls();

            bool hasItem = stack != null;
            m_ItemId = hasItem ? stack.ItemId : 0;

            if (m_Icon != null)
            {
                // m_Icon.color = hasItem ? Color.white : new Color(1f, 1f, 1f, 0.18f);
            }

            if (m_CountText != null)
            {
                m_CountText.text = hasItem ? Utility.Text.Format("{0}", stack.Count.ToString()) : string.Empty;
            }

            if (m_NameText != null)
            {
                m_NameText.text = hasItem ? GameEntry.Localization.GetString(stack.NameKey) : string.Empty;
            }
        }

        private void CacheControls()
        {
            if (m_Icon == null)
            {
                Transform iconTransform = transform.Find("Image");
                m_Icon = iconTransform != null ? iconTransform.GetComponent<Image>() : GetComponentInChildren<Image>(true);
            }

            if (m_CountText == null)
            {
                Transform countTransform = transform.Find("Count");
                m_CountText = countTransform != null ? countTransform.GetComponent<Text>() : GetComponentInChildren<Text>(true);
            }

            if (m_NameText == null)
            {
                Transform nameTransform = transform.Find("Name");
                m_NameText = nameTransform != null ? nameTransform.GetComponent<Text>() : null;
            }
        }
    }
}
