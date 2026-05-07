using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<KSG.PlayerLogic>;

namespace KSG
{
    public class PlayerIdleState : FsmState<PlayerLogic>, IReference
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
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            owner.desiredVelocity = Vector3.zero;
            owner.PlayAnimation(owner.playerAnimationName.SpeedParameterHash, 0f);

            // TODO : 建议 状态多了之后 使用 状态优先级 来处理状态切换
            if (owner.dashRequested)
            {
                ChangeState<PlayerDashState>(procedureOwner);
                return;
            }

            if (owner.playerMoveInput == Vector2.zero)
            {
                return;
            }
            
            ChangeState<PlayerMoveState>(procedureOwner);
            return;
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }
        public static PlayerIdleState Create()
        {
            PlayerIdleState state = ReferencePool.Acquire<PlayerIdleState>();
            return state;
        }
        public void Clear()
        {
            owner = null;
        }
    }
}
