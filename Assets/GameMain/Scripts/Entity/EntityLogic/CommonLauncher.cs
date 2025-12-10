using System;
using UnityEngine;

namespace KSG
{
    public class CommonLauncher : Launcher
    {
        /// <summary>
        /// 发射投射物
        /// </summary>
        /// <param name="attackerData">角色数据</param>
        /// <param name="origin">特效生成位置</param>
        /// <param name="firingPoint">发射点</param>
        public override void Launch(AttackerData attackerData, Vector3 origin, Transform firingPoint)
        {
            base.Launch(attackerData, origin, firingPoint);

            GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(
                TypeUtility.GetEntityType(attackerData.ProjectileType),
                "Projectile",
                "Projectile",
                Constant.AssetPriority.EntityAsset,
                null,
                EntityDataProjectile.Create(
                    GameEntry.Entity.GenerateSerialId(),
                    attackerData.ProjectileEntityId,
                    attackerData.AttackerId,
                    attackerData.CampType,
                    origin,
                    firingPoint
                )
            ));
        }
    }
}