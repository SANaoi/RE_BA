using UnityEngine;
using UnityGameFramework.Runtime;

namespace KSG
{
    public class ProjectileLogicRifle : ProjectileLogic
    {
        protected override void CheckHit(float distance)
        {
            if (Physics.Raycast(CachedTransform.position, m_LastPosition, out RaycastHit hit, distance, PlayerConstantData.CrosshairData.TargetLayerMask))
            {
                CachedTransform.position = hit.point;
                OnHit(hit);
            }
        }
        protected override void OnHit(RaycastHit hit)
        {
            if (hit.collider != null)
            {
                // 直接尝试获取 EnitityTargetable 组件
                // 假如Collider在子节点（如头部），GetComponentInParent可以向上查找
                EnitityTargetable target = hit.collider.GetComponentInParent<EnitityTargetable>();
                // 计算特效位置：击中点 + 沿法线稍微偏移一点
                Vector3 effectPosition = hit.point + hit.normal * 0.01f;

                // 计算特效旋转：面向法线方向（即从表面射出）
                Quaternion effectRotation = Quaternion.LookRotation(hit.normal);

                SpawnCollisionParticles(effectPosition, effectRotation);
                if (target != null)
                {
                    // 调用 EnitityTargetable 中定义的 TakeDamage
                    target.TakeDamage(projectileData.Damage);
                }
                else
                {
                    // 击中墙壁或其他物体
                }
                GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(Entity.Id));
            }
        }
    }
}