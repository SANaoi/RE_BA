using System;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

namespace KSG
{
    public class ProcedureLevel : ProcedureBase
    {
        private IFsm<IProcedureManager> m_ProcedureOwner;
        private LevelControl m_LevelControl;
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

            Log.Debug("Enter Game Procedure");
            m_LevelControl = LevelControl.Create();

            GameEntry.Event.Subscribe(ShowEntityInLevelEventArgs.EventId, OnShowEntityInLevel);
            
            
            this.m_ProcedureOwner = procedureOwner; 
            m_LevelControl.OnEnter();
            GameEntry.Entity.ShowPlayer(new EneityDataPlayer(GameEntry.Entity.GenerateSerialId(), 1001));
        }

        private void OnShowEntityInLevel(object sender, GameEventArgs e)
        {
            ShowEntityInLevelEventArgs ne = (ShowEntityInLevelEventArgs) e;
            if (ne == null)
            {
                return;
            }

            m_LevelControl.ShowEntity(ne.type, ne.entityGroup, ne.entityfolder, ne.priority, ne.ShowSuccess, ne.entityData);
        }
    }
}