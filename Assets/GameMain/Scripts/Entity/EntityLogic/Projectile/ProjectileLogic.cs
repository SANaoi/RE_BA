using UnityEngine;
using UnityGameFramework.Runtime;

namespace KSG
{
    public abstract class ProjectileLogic : EntityLogicBase
    {
        protected EntityDataProjectile projectileData;
        protected TrailRenderer m_trailRenderer;
        private float m_distanceTraveled = 0f;
        protected float m_lifeTime = 3f;
        protected float m_elapsedTime = 0f;
        protected Vector3 m_LastPosition;
        // TODO :
        // attackerData.Spread = spread;
        // attackerData.FireRate = fireRate;
        // attackerData.IsMultiAttack = isMultiAttack;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            projectileData = userData as EntityDataProjectile;
            if (projectileData == null)
            {
                Log.Error("Entity EntityProjectile '{0}' entity data invalid.", Id);
                return;
            }
            m_trailRenderer = GetComponent<TrailRenderer>();
            if (m_trailRenderer != null) m_trailRenderer.Clear();


            // 在 OnShow 中设置位置，确保每次复用都生效
            CachedTransform.position = projectileData.Position;
            CachedTransform.rotation = projectileData.Rotation;
            GetLaunchDirectionToSceneCenter();
            m_elapsedTime = 0f;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            m_distanceTraveled += projectileData.Speed * elapseSeconds;
            CheckHit(m_distanceTraveled);
            CachedTransform.position += m_LastPosition * m_distanceTraveled;

            m_elapsedTime += elapseSeconds;

            if (m_elapsedTime >= m_lifeTime)
            {
                m_elapsedTime = 0f;
                GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(Entity.Id));
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            projectileData = null;
            m_elapsedTime = 0f;
            m_distanceTraveled = 0f;
        }

        protected virtual void CheckHit(float distanceTraveled) { }

        protected virtual void OnHit(Vector3 hitPoint, Collider directHitCollider) { }
        protected virtual void OnHit(RaycastHit hit) { }

        private Vector3 GetLaunchDirectionToSceneCenter()
        {
            Ray camRay = Camera.main.ScreenPointToRay(
                new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));

            Vector3 aimPoint;
            if (Physics.Raycast(camRay, out RaycastHit camHit, 1000f))
                aimPoint = camHit.point;
            else
                aimPoint = camRay.origin + camRay.direction * 1000f;

            bool muzzleInside = Physics.OverlapSphere(
                CachedTransform.position,
                0.02f,
                PlayerConstantData.CrosshairData.TargetLayerMask
            ).Length > 0;

            if (muzzleInside)
            {
                return m_LastPosition = camRay.direction;
            }

            return m_LastPosition = (aimPoint - CachedTransform.position).normalized;
        }
        protected void SpawnCollisionParticles(Vector3 pos, Quaternion rotation)
        {
            if (projectileData.HitEffectId != 0)
            {
                GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create
                (
                    typeof(ParticleAutoHideLogic),
                    "Effect",
                    "Effect",
                    Constant.AssetPriority.EntityAsset,
                    null,
                    EntityData.Create(GameEntry.Entity.GenerateSerialId(), projectileData.HitEffectId, pos, rotation))
                );
            }
        }

    }
}