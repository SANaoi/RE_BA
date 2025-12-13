using UnityEngine;
using UnityGameFramework.Runtime;

namespace KSG
{
    public class ProjectileLogicRifle : ProjectileLogic
    {
        protected override void CheckHit(float distance)
        {
            if (Physics.Raycast(CachedTransform.position, launchDirection, out RaycastHit hit, distance, TargetLayerMask))
            {
                CachedTransform.position = hit.point;
                OnHit(hit.point, hit.collider);
            }
        }
        protected override void OnHit(Vector3 hitPoint, Collider directHitCollider)
        {
            if (directHitCollider != null)
            {
                // 直接尝试获取 EnitityTargetable 组件
                // 假如Collider在子节点（如头部），GetComponentInParent可以向上查找
                EnitityTargetable target = directHitCollider.GetComponentInParent<EnitityTargetable>();

                if (target != null)
                {
                    // 调用 EnitityTargetable 中定义的 TakeDamage
                    target.TakeDamage(projectileData.Damage);
                    Log.Info("击中目标 {0}，造成伤害 {1}", target.Name, projectileData.Damage);
                }
                else
                {
                    // 击中墙壁或其他物体
                    Log.Info("击中无效目标: " + directHitCollider.name);
                }
                GameEntry.Entity.HideEntity(this);
            }
        }
    }
}