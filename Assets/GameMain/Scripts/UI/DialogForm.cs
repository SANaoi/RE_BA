using UnityEngine;
using UnityGameFramework.Runtime;

namespace KSG
{
	public class DialogForm : UGuiForm
	{
		[SerializeField]
		private GameObject m_QuitButton = null;

		private ProcedureMenu m_ProcedureMenu = null;

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);

			m_ProcedureMenu = (ProcedureMenu)userData;
			if (m_ProcedureMenu == null)
			{
				Log.Warning("ProcedureMenu is invalid when open MenuForm.");
				return;
			}

			m_QuitButton.SetActive(Application.platform != RuntimePlatform.IPhonePlayer);
		}

		protected override void OnClose(bool isShutdown, object userData)
		{
            m_ProcedureMenu = null;

            base.OnClose(isShutdown, userData);
		}
	}
}