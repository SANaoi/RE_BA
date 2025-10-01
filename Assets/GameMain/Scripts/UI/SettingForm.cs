using GameFramework.Localization;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace KSG
{
    public class SettingForm : UGuiForm
    {

        [SerializeField]
        private CanvasGroup m_LanguageTipsCanvasGroup = null;

        [SerializeField]
        private Toggle m_EnglishToggle = null;

        [SerializeField]
        private Toggle m_ChineseToggle = null;

        [SerializeField]
        private Toggle m_JapaneseToggle = null;

        private Language m_SelectedLanguage = Language.Unspecified;

        public void OnSubmitButtonClick()
        {
            if (m_SelectedLanguage == GameEntry.Localization.Language)
            {
                Close();
                return;
            }


            GameEntry.Setting.SetString(Constant.Setting.Language, m_SelectedLanguage.ToString());
            GameEntry.Setting.Save();

            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Restart);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_SelectedLanguage = GameEntry.Localization.Language;
            switch (m_SelectedLanguage)
            {
                case Language.English:
                    m_EnglishToggle.isOn = true;
                    break;

                case Language.ChineseSimplified:
                    m_ChineseToggle.isOn = true;
                    break;
                case Language.Japanese:
                    m_JapaneseToggle.isOn = true;
                    break;

                default:
                    break;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (m_LanguageTipsCanvasGroup.gameObject.activeSelf)
            {
                m_LanguageTipsCanvasGroup.alpha = 0.5f + 0.5f * Mathf.Sin(Mathf.PI * Time.time);
            }
        }

        public void OnEnglishSelected(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            m_SelectedLanguage = Language.English;
            RefreshLanguageTips();
        }

        public void OnChineseSelected(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            m_SelectedLanguage = Language.ChineseSimplified;
            RefreshLanguageTips();
        }

        public void OnJapaneseSelected(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            m_SelectedLanguage = Language.Japanese;
            RefreshLanguageTips();
        }

        private void RefreshLanguageTips()
        {
            m_LanguageTipsCanvasGroup.gameObject.SetActive(m_SelectedLanguage != GameEntry.Localization.Language);
        }
    }
}