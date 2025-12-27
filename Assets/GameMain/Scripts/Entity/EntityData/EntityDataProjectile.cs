using GameFramework;
using GameFramework.DataTable;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace KSG
{
    public class EntityDataProjectile : EntityData
    {
        [SerializeField]
        private int m_OwnerId = 0;

        [SerializeField]
        private CampType m_OwnerCamp = CampType.Unknown;

        [SerializeField]
        private float m_Damage = 0f;

        [SerializeField]
        private float m_Speed = 0f;

        [SerializeField]
        private float m_SplashDamage = 0f;

        [SerializeField]
        private float m_SplashRange = 0f;
        [SerializeField]
        private Vector3 m_Origin = Vector3.zero;
        [SerializeField]
        private int m_HitEffectId = 0;
        public static EntityDataProjectile Create
        (
            int entityId, 
            int typeId, 
            int ownerId, 
            CampType ownerCamp,
            Vector3 origin, 
            Transform firingPoint
            )
        {
            EntityDataProjectile data = ReferencePool.Acquire<EntityDataProjectile>();
            data.Id = entityId;
            data.TypeId = typeId;
            data.m_OwnerId = ownerId;
            data.m_OwnerCamp = ownerCamp;
            data.m_Origin = origin;
            data.Position = firingPoint.transform.position;
            data.Rotation = firingPoint.transform.rotation;
            
            IDataTable<DRProjectile> dt = GameEntry.DataTable.GetDataTable<DRProjectile>();
            DRProjectile row = dt.GetDataRow(typeId);
            if (row != null)
            {
                data.m_Damage = row.Damage;
                data.m_Speed = row.Speed;
                data.m_HitEffectId = row.HitEffectId;
                data.m_SplashDamage = row.SplashDamage;
                data.m_SplashRange = row.SplashRange;
            }
            else
            {
                Log.Warning("Can not find projectile id '{0}'.", typeId);
            }
            return data;
        }

        public override void Clear()
        {
            base.Clear();
            m_OwnerId = 0;
            m_OwnerCamp = CampType.Unknown;
            m_Damage = 0f;
            m_Speed = 0f;
            m_SplashDamage = 0f;
            m_SplashRange = 0f;
        }
        /// <summary>
        /// 获取拥有者实体编号。
        /// </summary>
        public int OwnerId
        {
            get
            {
                return m_OwnerId;
            }
        }

        /// <summary>
        /// 获取拥有者阵营。
        /// </summary>
        public CampType OwnerCamp
        {
            get
            {
                return m_OwnerCamp;
            }
        }

        /// <summary>
        /// 获取伤害。
        /// </summary>
        public float Damage
        {
            get
            {
                return m_Damage;
            }
        }

        /// <summary>
        /// 获取射速。
        /// </summary>
        public float Speed
        {
            get
            {
                return m_Speed;
            }
        }

        /// <summary>
        /// 获取溅射伤害。
        /// </summary>
        public float SplashDamage
        {
            get
            {
                return m_SplashDamage;
            }
        }

        /// <summary>
        /// 获取溅射范围。
        /// </summary>
        public float SplashRange
        {
            get
            {
                return m_SplashRange;
            }
        }
        public Vector3 Origin
        {
            get
            {
                return m_Origin;
            }
            set
            {
                m_Origin = value;
            }
        }
        public int HitEffectId
        {
            get
            {
                return m_HitEffectId;
            }
            set
            {
                m_HitEffectId = value;
            }
        }
    }
}