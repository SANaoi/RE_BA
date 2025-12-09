using System;
using UnityEngine;

namespace KSG
{
    public class CommonLauncher : Launcher
    {
        public override void Launch(AttackerData attackerData, Vector3 origin, Transform firingPoint)
        {
            base.Launch(attackerData, origin, firingPoint);

            GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(
                Type.GetType(attackerData.ProjectileType),
                "Projectile",
                "Projectile",
                Constant.AssetPriority.EntityAsset,
                null,
                EntityDataProjectile.Create(
                    GameEntry.Entity.GenerateSerialId(),
                    attackerData.ProjectileEntityId,
                    attackerData.AttackerId,
                    attackerData.CampType
                )
            ));

            //TODO : 接下来实现发射逻辑，如发送位置，速度
        }
    }
}