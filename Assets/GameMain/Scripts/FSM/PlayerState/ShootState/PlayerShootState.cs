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
        private float shootDalyTime = 0.2f;
        private float shootTimer = 0f;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            owner.StartAnimation(owner.playerAnimationName.isShootParameterName);
            
            owner.targetAimRiggingWeight = 1f;
            owner.targetWeaponRiggingWeight = 1f;

            shootTimer = shootDalyTime; //进入状态时立即开火
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
            shootTimer += elapseSeconds;
            if (shootTimer >= shootDalyTime)
            {
                Shoot();
                shootTimer = 0f;
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

        private void Shoot()
        {
            owner.playerLauncher.Launch(owner.attackerData, owner.playerLauncher.firingPoint.position, owner.playerLauncher.firingPoint);
        }
    }
}