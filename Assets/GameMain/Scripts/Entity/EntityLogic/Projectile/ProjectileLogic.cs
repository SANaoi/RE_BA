using UnityEngine;
using UnityGameFramework.Runtime;

namespace KSG
{
    public abstract class ProjectileLogic : EntityBase
    {
        protected EntityDataProjectile projectileData;
        protected const int TargetLayerMask = 1 << 8;
        private float speed;
        private float distanceTraveled = 0f;
        protected float lifeTime = 3f;
        protected float elapsedTime = 0f;
        protected Vector3 launchDirection;
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

            // 在 OnShow 中设置位置，确保每次复用都生效
            CachedTransform.position = projectileData.Position;
            CachedTransform.rotation = projectileData.Rotation;
            speed = projectileData.Speed;
            GetLaunchDirectionToSceneCenter();

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            distanceTraveled += speed * elapseSeconds;
            CheckHit(distanceTraveled);
            CachedTransform.position += launchDirection * distanceTraveled;

            elapsedTime += elapseSeconds;

            if (elapsedTime >= lifeTime)
            {
                elapsedTime = 0f;
                GameEntry.Entity.HideEntity(this);
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            projectileData = null;
            elapsedTime = 0f;
            distanceTraveled = 0f;
        }

        protected virtual void CheckHit(float distanceTraveled) { }

        protected virtual void OnHit(Vector3 hitPoint, Collider directHitCollider) { }
        private Vector3 GetLaunchDirectionToSceneCenter()
        {
            Ray ray = Camera.main.ScreenPointToRay(
            new Vector3(Screen.width / 2f, Screen.height / 2f));

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.origin + ray.direction * 100f;
            }
            launchDirection = (targetPoint - CachedTransform.position).normalized;
            return launchDirection;
        }
        private void SpawnCollisionParticles(Vector3 hitPoint)
        {
            //TODO : 播放粒子特效
        }

    }
}