using UnityEngine;
using UnityGameFramework.Runtime;

namespace KSG
{
    public class MenuForm : UGuiFormEx
    {

        private ProcedureMenu m_ProcedureMenu = null;

        public void OnSettingButtonClick()
        {
            GameEntry.UI.OpenUIForm(EnumUIForm.SettingForm);
        }

        public void OnStartButtonClick()
        {
            m_ProcedureMenu.StartGame();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_ProcedureMenu = (ProcedureMenu)userData;
            if (m_ProcedureMenu == null)
            {
                Log.Warning("ProcedureMenu is invalid when open MenuForm.");
                return;
            }

            // m_QuitButton.SetActive(Application.platform != RuntimePlatform.IPhonePlayer)
        }
	}
}