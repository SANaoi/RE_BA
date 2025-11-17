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
                owner.transform.Translate(new Vector3(owner.playerMoveInput.x, 0, owner.playerMoveInput.y) * (owner.playerData.Speed * (float)0.5) * Time.deltaTime);
                owner.PlayAnimation(owner.playerAnimationName.SpeedParameterHash, owner.playerMoveInput.magnitude * (float)0.5);}
            else
            {
                owner.transform.Translate(Vector3.forward * (owner.playerData.Speed * (float)0.7) * Time.deltaTime);
                owner.PlayAnimation(owner.playerAnimationName.SpeedParameterHash, owner.playerMoveInput.magnitude * (float)0.7);
            }

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
        }
        public static PlayerMoveState Create()
        {
            PlayerMoveState state = ReferencePool.Acquire<PlayerMoveState>();
            return state;
        }
        public void Clear()
        {
            owner = null;
        }
    }
}