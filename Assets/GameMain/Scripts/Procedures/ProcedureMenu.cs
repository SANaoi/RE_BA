using System;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

namespace KSG
{
    public class ProcedureMenu : ProcedureBase
    {
        private bool m_StartGame = false;
        private int MenuFormSerSerialId;
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Debug("Enter Menu Procedure");
            m_StartGame = false;
            MenuFormSerSerialId = (int)GameEntry.UI.OpenUIForm(EnumUIForm.MenuForm, this);
            Log.Debug("MenuFormSerSerialId : " + MenuFormSerSerialId);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_StartGame)
            {
                GameEntry.DataNode.GetOrAddNode(Constant.ProcedureRunningData.NextSceneName).SetData<VarString>("Game");
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            if (MenuFormSerSerialId != 0)
            {
                GameEntry.UI.CloseUIForm(MenuFormSerSerialId);
            }
        }
        public void StartGame()
        {
            m_StartGame = true;
        }
    }
}