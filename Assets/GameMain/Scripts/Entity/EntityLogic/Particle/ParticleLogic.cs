using UnityEngine;

namespace KSG
{
    public class ParitcleLogic : EntityLogicBase
    {
        private EntityDataFollower entityDataFollower;
        protected ParticleSystem ps;
        private float pauseTime;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            ps = GetComponentInChildren<ParticleSystem>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            entityDataFollower = userData as EntityDataFollower;
            if (entityDataFollower == null)
            {
                return;
            }

            transform.localScale = entityDataFollower.Scale;
            ps.Play(true);

        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            if (entityDataFollower != null && entityDataFollower.FollowTarget != null)
            {
                transform.position = entityDataFollower.FollowTarget.position + entityDataFollower.Offset;
            }
        }
        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            entityDataFollower = null;

            transform.localScale = Vector3.one;
            pauseTime = 0;
            ps.Stop(true);
        }

        public override void Pause()
        {
            pause = true;
            ps.Pause(true);
            pauseTime = ps.time;
        }

        public override void Resume()
        {
            pause = false;
            ps.Play();
            ps.time = pauseTime;
        }
        protected override void OnRecycle()
        {
            base.OnRecycle();
        }
    }
}