using UnityGameFramework.Runtime;

namespace KSG
{
    public abstract class ProjectileLogic : EntityBase
    {
        protected EntityDataProjectile projectileData;
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
            
            // 1. 每次显示时，重新获取数据（因为复用时 userData 会变）
            projectileData = userData as EntityDataProjectile;

            if (projectileData == null)
            {
                Log.Error("Entity EntityProjectile '{0}' entity data invalid.", Id);
                return;
            }

            // 2. 在 OnShow 中设置位置，确保每次复用都生效
            if (projectileData.FiringPoint != null)
            {
                CachedTransform.position = projectileData.FiringPoint.position;
                CachedTransform.rotation = projectileData.FiringPoint.rotation; // 通常也需要设置旋转
            }
            else
            {
                // 如果没有Transform引用，使用数据中的坐标字段（推荐做法）
                CachedTransform.position = projectileData.Origin;
            }
        }

        //TODO : SpawnCollisionParticles
    }
}