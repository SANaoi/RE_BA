using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<KSG.PlayerLogic>;

namespace KSG
{
    public class PlayerShootState : FsmState<PlayerLogic>, IReference
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
            owner.StartAnimation(owner.playerAnimationName.isShootParameterName);

        }

        protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            if (!owner.isAim && !owner.isShoot)
            {
                ChangeState<PlayerNormalState>(fsm);
            }
            else if (owner.isAim && !owner.isShoot)
            {
                ChangeState<PlayerAimState>(fsm);
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.StopAnimation(owner.playerAnimationName.isShootParameterName);

        }

        protected override void OnDestroy(ProcedureOwner fsm)
        {
            base.OnDestroy(fsm);


        }

        public static PlayerShootState Create()
        {
            PlayerShootState state = ReferencePool.Acquire<PlayerShootState>();
            return state;
        }
        public void Clear()
        {
            owner = null;
        }
    }
}