using System;
using GameFramework.Event;
using GameFramework.Localization;
using UnityGameFramework.Runtime;

namespace KSG
{
    public static class LanguageSwitcher
    {
        private const string LanguageSwitchUserData = "LanguageSwitch";

        private static bool s_IsSwitching;
        private static Language s_TargetLanguage = Language.Unspecified;

        public static bool IsSwitching => s_IsSwitching;

        public static void SwitchLanguage(Language language)
        {
            if (s_IsSwitching || language == Language.Unspecified || language == GameEntry.Localization.Language)
            {
                return;
            }

            s_IsSwitching = true;
            s_TargetLanguage = language;

            GameEntry.Event.Subscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
            GameEntry.Event.Subscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

            GameEntry.Setting.SetString(Constant.Setting.Language, language.ToString());
            GameEntry.Setting.Save();

            GameEntry.Localization.RemoveAllRawStrings();
            GameEntry.Localization.Language = language;
            SetCurrentVariant(language);

            // Match the startup sequence so hot-switch and cold-start stay consistent.
            GameEntry.BuiltinData.InitDefaultDictionary();
            GameEntry.Localization.ReadData(AssetUtility.GetDictionaryAsset("Default"), LanguageSwitchUserData);
        }

        private static void OnLoadDictionarySuccess(object sender, GameEventArgs e)
        {
            LoadDictionarySuccessEventArgs ne = (LoadDictionarySuccessEventArgs)e;
            if (!s_IsSwitching || !Equals(ne.UserData, LanguageSwitchUserData))
            {
                return;
            }

            Language switchedLanguage = s_TargetLanguage;
            ClearSwitchState();
            GameEntry.Event.Fire(null, LanguageChangedEventArgs.Create(switchedLanguage));
        }

        private static void OnLoadDictionaryFailure(object sender, GameEventArgs e)
        {
            LoadDictionaryFailureEventArgs ne = (LoadDictionaryFailureEventArgs)e;
            if (!s_IsSwitching || !Equals(ne.UserData, LanguageSwitchUserData))
            {
                return;
            }

            Log.Warning(
                "Language switch failed when loading dictionary '{0}': {1}",
                ne.DictionaryAssetName,
                ne.ErrorMessage
            );

            ClearSwitchState();
        }

        private static void ClearSwitchState()
        {
            GameEntry.Event.Unsubscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
            GameEntry.Event.Unsubscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

            s_IsSwitching = false;
            s_TargetLanguage = Language.Unspecified;
        }

        private static void SetCurrentVariant(Language language)
        {
            if (GameEntry.Base.EditorResourceMode)
            {
                return;
            }

            string currentVariant;
            switch (language)
            {
                case Language.English:
                    currentVariant = "en-us";
                    break;

                case Language.ChineseSimplified:
                    currentVariant = "zh-cn";
                    break;

                case Language.Japanese:
                    currentVariant = "ja-jp";
                    break;

                default:
                    currentVariant = "zh-cn";
                    break;
            }

            GameEntry.Resource.SetCurrentVariant(currentVariant);
            
        }
    }
}
