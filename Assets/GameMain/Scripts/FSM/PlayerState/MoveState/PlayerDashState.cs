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


        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            owner.StartCoroutine(Dash(procedureOwner));
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
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
        }

        private IEnumerator Dash(ProcedureOwner procedureOwner)
        {
            if (owner.isDashing == false)
            {
                Log.Error("Dash State Entered but isDashing is false");
                yield break;
            }
            float endTime = Time.time + PlayerConstantData.DashData.DASHDURATION;

            while (Time.time < endTime)
            {
                if (owner.isAimOrShootState)
                {
                    owner.transform.Translate(new Vector3(owner.playerMoveInput.x, 0, owner.playerMoveInput.y) * PlayerConstantData.DashData.DASHSPEED * Time.deltaTime);
                    yield return null;
                }
                else
                {
                    owner.transform.Translate(
                    Vector3.forward * PlayerConstantData.DashData.DASHSPEED * Time.deltaTime);
                    yield return null;
                }
            }

            owner.isDashing = false;
            owner.lastDashTime = Time.time;
            if (owner.playerMoveInput == Vector2.zero)
            {
                ChangeState<PlayerIdleState>(procedureOwner);
            }
            ChangeState<PlayerMoveState>(procedureOwner);

        }
    }
}