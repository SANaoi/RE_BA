using UnityEngine;
using UnityGameFramework.Runtime;

namespace KSG
{
    public class ProjectileLogicRifle : ProjectileLogic
    {
        protected override void CheckHit(float distance)
        {
            if (Physics.Raycast(CachedTransform.position, launchDirection, out RaycastHit hit, distance, PlayerConstantData.CrosshairData.TargetLayerMask))
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
                }
                else
                {
                    // 击中墙壁或其他物体
                }
                GameEntry.Entity.HideEntity(this);
            }
        }
    }
}