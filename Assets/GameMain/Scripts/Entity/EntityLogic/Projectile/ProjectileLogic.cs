using UnityGameFramework.Runtime;

namespace KSG
{
    public abstract class ProjectileLogic : EntityBase
    {
        protected EntityDataProjectile projectileData;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            projectileData = userData as EntityDataProjectile;

            if (projectileData == null)
            {
                Log.Error("Entity EntityProjectile '{0}' entity data invaild.", Id);
                return;
            }
        }

        //TODO : SpawnCollisionParticles
    }
}