using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<KSG.PlayerLogic>;

namespace KSG
{
    public class PlayerAimState : FsmState<PlayerLogic>, IReference
    {
        private PlayerLogic owner;
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            owner.StartAnimation(owner.playerAnimationName.isAimParameterName);
            Log.Debug("PlayerAimState");
        }

        protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

            if (!owner.isAim && !owner.isShoot)
            {
                ChangeState<PlayerNormalState>(fsm);
            }
            else if (owner.isShoot)
            {
                ChangeState<PlayerShootState>(fsm);
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }

        protected override void OnDestroy(ProcedureOwner fsm)
        {
            base.OnDestroy(fsm);


        }

        public static PlayerAimState Create()
        {
            PlayerAimState state = ReferencePool.Acquire<PlayerAimState>();
            return state;
        }
        public void Clear()
        {
            owner = null;
        }
    }
}