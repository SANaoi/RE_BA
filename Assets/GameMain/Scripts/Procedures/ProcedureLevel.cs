using System;
using GameFramework;
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
            GameEntry.Event.Subscribe(HideEntityInLevelEventArgs.EventId, OnHideEntityInLevel);
            
            this.m_ProcedureOwner = procedureOwner; 
            m_LevelControl.OnEnter();
            
        }
        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(ShowEntityInLevelEventArgs.EventId, OnShowEntityInLevel);
            GameEntry.Event.Unsubscribe(HideEntityInLevelEventArgs.EventId, OnHideEntityInLevel);

            m_LevelControl.Quick();
            ReferencePool.Release(m_LevelControl);
            m_LevelControl = null;
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

        private void OnHideEntityInLevel(object sender, GameEventArgs e)
        {
            HideEntityInLevelEventArgs ne = (HideEntityInLevelEventArgs) e;
            if (ne == null)
            {
                return;
            }

            m_LevelControl.HideEntity(ne.EntityId);
        }
    }
}