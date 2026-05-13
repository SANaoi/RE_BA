using UnityGameFramework.Runtime;

namespace KSG
{
    public class BagForm : UGuiFormEx
    {
        private ProcedureLevel m_ProcedureLevel = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            UIWindowController.RegisterForm(EnumUIForm.BagForm, true, true, true);
            m_ProcedureLevel = userData as ProcedureLevel;
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            UIWindowController.UnregisterForm(EnumUIForm.BagForm);
            base.OnClose(isShutdown, userData);
        }
    }
}
