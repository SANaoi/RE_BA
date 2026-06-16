//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace KSG
{
	public abstract class UGuiForm : UIFormLogic
	{
		public const int DepthFactor = 100;
		private const float FadeTime = 0.3f;

		private static Font s_MainFont = null;
		private static TMP_FontAsset s_MainTmpFont = null;
		private Canvas m_CachedCanvas = null;
		private CanvasGroup m_CanvasGroup = null;
		private List<Canvas> m_CachedCanvasContainer = new List<Canvas>();
		private readonly Dictionary<Text, string> m_LocalizedKeys = new Dictionary<Text, string>();
		private readonly Dictionary<TMP_Text, string> m_LocalizedTmpKeys = new Dictionary<TMP_Text, string>();
		public int OriginalDepth
		{
			get;
			private set;
		}

		public int Depth
		{
			get
			{
				return m_CachedCanvas.sortingOrder;
			}
		}

		public void Close()
		{
			Close(false);
		}

		public void Close(bool ignoreFade)
		{
			StopAllCoroutines();

			if (ignoreFade)
			{
				GameEntry.UI.CloseUIForm(this);
			}
			else
			{
				StartCoroutine(CloseCo(FadeTime));
			}
		}

		// public void PlayUISound(int uiSoundId)
		// {
		//     GameEntry.Sound.PlayUISound(uiSoundId);
		// }

		public static void SetMainFont(Font mainFont)
		{
			if (mainFont == null)
			{
				Log.Error("Main font is invalid.");
				return;
			}

			s_MainFont = mainFont;
		}

		public static void SetMainTmpFont(TMP_FontAsset mainTmpFont)
		{
			if (mainTmpFont == null)
			{
				Log.Error("Main TMP font is invalid.");
				return;
			}

			s_MainTmpFont = mainTmpFont;
		}

		protected virtual void RefreshLocalization()
		{
			foreach (var kvp in m_LocalizedKeys)
			{
				if (kvp.Key != null)
				{
					kvp.Key.text = GameEntry.Localization.GetString(kvp.Value);
				}
			}

			foreach (var kvp in m_LocalizedTmpKeys)
			{
				if (kvp.Key != null)
				{
					kvp.Key.text = GameEntry.Localization.GetString(kvp.Value);
				}
			}
		}
#if UNITY_2017_3_OR_NEWER
		protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
		{
			base.OnInit(userData);

			m_CachedCanvas = gameObject.GetOrAddComponent<Canvas>();
			m_CachedCanvas.overrideSorting = true;
			OriginalDepth = m_CachedCanvas.sortingOrder;

			m_CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();

			RectTransform transform = GetComponent<RectTransform>();
			transform.anchorMin = Vector2.zero;
			transform.anchorMax = Vector2.one;
			transform.anchoredPosition = Vector2.zero;
			transform.sizeDelta = Vector2.zero;

			gameObject.GetOrAddComponent<GraphicRaycaster>();

			m_LocalizedKeys.Clear();
			m_LocalizedTmpKeys.Clear();
			Text[] texts = GetComponentsInChildren<Text>(true);
			for (int i = 0; i < texts.Length; i++)
			{
				texts[i].font = s_MainFont;
				if (!string.IsNullOrEmpty(texts[i].text))
				{
					m_LocalizedKeys[texts[i]] = texts[i].text;
					texts[i].text = GameEntry.Localization.GetString(texts[i].text);
				}
			}

			TMP_Text[] tmpTexts = GetComponentsInChildren<TMP_Text>(true);
			for (int i = 0; i < tmpTexts.Length; i++)
			{
				if (s_MainTmpFont != null)
				{
					tmpTexts[i].font = s_MainTmpFont;
				}

				if (!string.IsNullOrEmpty(tmpTexts[i].text))
				{
					m_LocalizedTmpKeys[tmpTexts[i]] = tmpTexts[i].text;
					tmpTexts[i].text = GameEntry.Localization.GetString(tmpTexts[i].text);
				}
			}
		}

#if UNITY_2017_3_OR_NEWER
		protected override void OnRecycle()
#else
        protected internal override void OnRecycle()
#endif
		{
			base.OnRecycle();
		}

#if UNITY_2017_3_OR_NEWER
		protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
		{
			base.OnOpen(userData);
			RefreshLocalization();

			PlayFadeIn();
		}

#if UNITY_2017_3_OR_NEWER
		protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
		{
			base.OnClose(isShutdown, userData);
		}

#if UNITY_2017_3_OR_NEWER
		protected override void OnPause()
#else
        protected internal override void OnPause()
#endif
		{
			base.OnPause();
		}

#if UNITY_2017_3_OR_NEWER
		protected override void OnResume()
#else
        protected internal override void OnResume()
#endif
		{
			base.OnResume();

			PlayFadeIn();
		}

#if UNITY_2017_3_OR_NEWER
		protected override void OnCover()
#else
        protected internal override void OnCover()
#endif
		{
			base.OnCover();
		}

#if UNITY_2017_3_OR_NEWER
		protected override void OnReveal()
#else
        protected internal override void OnReveal()
#endif
		{
			base.OnReveal();
		}

#if UNITY_2017_3_OR_NEWER
		protected override void OnRefocus(object userData)
#else
        protected internal override void OnRefocus(object userData)
#endif
		{
			base.OnRefocus(userData);
		}

#if UNITY_2017_3_OR_NEWER
		protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
		{
			base.OnUpdate(elapseSeconds, realElapseSeconds);
			UIWindowController.Tick();
		}

#if UNITY_2017_3_OR_NEWER
		protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
#else
        protected internal override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
#endif
		{
			int oldDepth = Depth;
			base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
			int deltaDepth = UGuiGroupHelper.DepthFactor * uiGroupDepth + DepthFactor * depthInUIGroup - oldDepth + OriginalDepth;
			GetComponentsInChildren(true, m_CachedCanvasContainer);
			for (int i = 0; i < m_CachedCanvasContainer.Count; i++)
			{
				m_CachedCanvasContainer[i].sortingOrder += deltaDepth;
			}

			m_CachedCanvasContainer.Clear();
		}

		private IEnumerator CloseCo(float duration)
		{
			yield return m_CanvasGroup.FadeToAlpha(0f, duration);
			GameEntry.UI.CloseUIForm(this);
		}

		private void PlayFadeIn()
		{
			if (m_CanvasGroup == null)
			{
				return;
			}

			m_CanvasGroup.alpha = 0f;

			if (!isActiveAndEnabled || !gameObject.activeInHierarchy)
			{
				return;
			}

			StopAllCoroutines();
			StartCoroutine(m_CanvasGroup.FadeToAlpha(1f, FadeTime));
		}
	}
}
