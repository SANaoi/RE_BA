using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<KSG.PlayerLogic>;

namespace KSG
{
    public class PlayerNormalState : FsmState<PlayerLogic>, IReference
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
            
            owner.StopAnimation(owner.playerAnimationName.isAimParameterName);
            owner.StopAnimation(owner.playerAnimationName.isShootParameterName);

            owner.targetAimRiggingWeight = 0f;
            owner.targetWeaponRiggingWeight = 0f;
        }

        protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            if (!owner.isAim && !owner.isShoot)
            {
                owner.isAimOrShootState = false;
                return;
            }
            else if (owner.isAim || owner.isShoot)
            {
                owner.isAimOrShootState = true;
                ChangeState<PlayerAimState>(fsm);
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

        public static PlayerNormalState Create()
        {
            PlayerNormalState state = ReferencePool.Acquire<PlayerNormalState>();
            return state;
        }
        public void Clear()
        {
            owner = null;
        }
    }
}