using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<KSG.PlayerLogic>;

namespace KSG
{
    public class PlayerDashState : FsmState<PlayerLogic>, IReference
    {
        private PlayerLogic owner;
        private Vector3 dashDir;
        private float dashEndTime;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            owner.dashRequested = false;
            owner.isDashing = true;
            owner.lastDashTime = Time.time;

            dashDir = ResolveDashDirection();
            dashEndTime = Time.time + PlayerConstantData.DashData.DASHDURATION;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            owner.desiredVelocity = dashDir * PlayerConstantData.DashData.DASHSPEED;

            if (Time.time < dashEndTime)
            {
                return;
            }

            owner.desiredVelocity = Vector3.zero;
            owner.isDashing = false;

            if (owner.playerMoveInput == Vector2.zero)
            {
                ChangeState<PlayerIdleState>(procedureOwner);
            }
            else if (owner.isRunning)
            {
                ChangeState<PlayerRunState>(procedureOwner);
            }
            else
            {
                ChangeState<PlayerMoveState>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            owner.desiredVelocity = Vector3.zero;
            owner.isDashing = false;
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }
        public static PlayerDashState Create()
        {
            PlayerDashState state = ReferencePool.Acquire<PlayerDashState>();
            return state;
        }
        public void Clear()
        {
            owner = null;
            dashDir = Vector3.zero;
            dashEndTime = 0f;
        }

        private Vector3 ResolveDashDirection()
        {
            if (owner.playerMoveInput == Vector2.zero)
            {
                return -owner.transform.forward;
            }

            if (owner.isAimOrShootState)
            {
                Vector3 forward = owner.transform.forward;
                Vector3 right = owner.transform.right;
                forward.y = 0f;
                right.y = 0f;

                forward.Normalize();
                right.Normalize();

                Vector3 inputDashDir =
                    right * owner.playerMoveInput.x +
                    forward * owner.playerMoveInput.y;

                if (inputDashDir.sqrMagnitude > 0.0001f)
                {
                    inputDashDir.Normalize();
                    return inputDashDir;
                }

                return -owner.transform.forward;
            }

            return owner.transform.forward;
        }
    }
}
