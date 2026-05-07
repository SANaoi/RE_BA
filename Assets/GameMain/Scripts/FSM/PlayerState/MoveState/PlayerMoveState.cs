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
        float animationSpeed;
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
            float inputMagnitude = Mathf.Clamp01(owner.playerMoveInput.magnitude);

            if (owner.isAimOrShootState)
            {
                Vector3 camForward = owner.m_Camera.transform.forward;
                Vector3 camRight = owner.m_Camera.transform.right;
                animationSpeed = owner.playerMoveInput.magnitude * 0.3f;
                camForward.y = 0f;
                camRight.y = 0f;

                camForward.Normalize();
                camRight.Normalize();

                dir =
                    camRight * owner.playerMoveInput.x +
                    camForward * owner.playerMoveInput.y;

                dir *= owner.playerData.Speed * 0.3f;
            }
            else
            {
                dir = owner.transform.forward * (owner.playerData.Speed * 0.4f * inputMagnitude);
                animationSpeed = dir.magnitude / owner.playerData.Speed;
            }
            
            owner.desiredVelocity = dir;
            if (owner.isAimOrShootState)
            {
                owner.PlayAnimation(
                    owner.playerAnimationName.SpeedParameterHash,
                    animationSpeed
                );
            }
            else
            {
                owner.PlayLocomotionAnimation(dir);
            }

            if (owner.dashRequested)
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
            animationSpeed = 0f;
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
            animationSpeed = 0f;
        }
    }
}
