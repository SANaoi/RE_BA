using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<KSG.PlayerLogic>;

namespace KSG
{
    public class PlayerMoveState : FsmState<PlayerLogic>, IReference
    {
        private PlayerLogic owner;
        Vector3 dir;
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

            if (owner.isAimOrShootState)
            {
                Vector3 camForward = owner.m_Camera.transform.forward;
                Vector3 camRight = owner.m_Camera.transform.right;

                camForward.y = 0f;
                camRight.y = 0f;

                camForward.Normalize();
                camRight.Normalize();

                dir =
                    camRight * owner.playerMoveInput.x +
                    camForward * owner.playerMoveInput.y;

                dir *= owner.playerData.Speed * 0.5f;
            }
            else
            {
                dir = owner.transform.forward * (owner.playerData.Speed * 0.7f);
            }

            owner.desiredVelocity = dir;
            owner.PlayAnimation(
                owner.playerAnimationName.SpeedParameterHash,
                owner.playerMoveInput.magnitude / owner.playerData.Speed
            );

            if (owner.isDashing)
            {
                ChangeState<PlayerDashState>(procedureOwner);
                return;
            }

            if (owner.isRunning && owner.playerMoveInput != Vector2.zero)
            {
                ChangeState<PlayerRunState>(procedureOwner);
            }
            //切换回空闲状态
            else if (owner.playerMoveInput == Vector2.zero)
            {
                ChangeState<PlayerIdleState>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            dir = Vector3.zero;
        }
        public static PlayerMoveState Create()
        {
            PlayerMoveState state = ReferencePool.Acquire<PlayerMoveState>();
            return state;
        }
        public void Clear()
        {
            owner = null;
            dir = Vector3.zero;
        }
    }
}