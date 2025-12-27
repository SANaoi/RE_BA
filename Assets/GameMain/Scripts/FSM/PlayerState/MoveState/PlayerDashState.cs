using System.Collections;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<KSG.PlayerLogic>;

namespace KSG
{
    public class PlayerDashState : FsmState<PlayerLogic>, IReference
    {
        private PlayerLogic owner;
        Vector3 dashDir;
        private Coroutine dashCoroutine;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            dashCoroutine = owner.StartCoroutine(Dash(procedureOwner));
            dashDir = Vector3.zero;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            if (dashCoroutine != null)
            {
                owner.StopCoroutine(dashCoroutine);
                dashCoroutine = null;
            }
            owner.desiredVelocity = Vector3.zero;
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
            dashCoroutine = null;
        }

        private IEnumerator Dash(ProcedureOwner procedureOwner)
        {
            if (owner.isDashing == false)
            {
                Log.Error("Dash State Entered but isDashing is false");
                yield break;
            }
            float endTime = Time.time + PlayerConstantData.DashData.DASHDURATION;

            if (owner.isAimOrShootState)
            {
                dashDir = new Vector3(
                    owner.playerMoveInput.x,
                    0f,
                    owner.playerMoveInput.y
                );

                if (dashDir.sqrMagnitude < 0.01f)
                {
                    dashDir = owner.transform.forward;
                }

                dashDir.Normalize();
            }
            else
            {
                dashDir = owner.transform.forward;
            }

            while (Time.time < endTime)
            {
                owner.desiredVelocity =
                    dashDir * PlayerConstantData.DashData.DASHSPEED;

                yield return null;
            }

            owner.desiredVelocity = Vector3.zero;
            owner.isDashing = false;
            owner.lastDashTime = Time.time;
            if (owner.playerMoveInput == Vector2.zero)
            {
                ChangeState<PlayerIdleState>(procedureOwner);
            }
            else
            {
                ChangeState<PlayerMoveState>(procedureOwner);
            }

        }
    }
}