using GameFramework;
using GameFramework.DataTable;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace KSG
{
    public class EntityDataPlayer : EntityDataCharacter
    {
        [SerializeField]
        private float m_MaxHP = 0;
        [SerializeField]
        private float m_Speed = 0;
        [SerializeField]
        private int m_CameraId = 0;
        [SerializeField]
        private int m_ProjectileEntityId = 0;
        [SerializeField]
        private string m_ProjectileType = "";
        public EntityDataPlayer()
        {
            OwnerCamp = CampType.Unknown;
            m_MaxHP = 0;
            m_Speed = 0;
            m_CameraId = 0;
        }


        public static EntityDataPlayer Create(int entityId, int typeId, CampType ownerCamp = CampType.Unknown)
        {
            EntityDataPlayer data = ReferencePool.Acquire<EntityDataPlayer>();
            data.Id = entityId;
            data.TypeId = typeId;
            IDataTable<DRPlayer> dt = GameEntry.DataTable.GetDataTable<DRPlayer>();
            DRPlayer row = dt.GetDataRow(typeId);
            if (row != null)
            {
                data.m_MaxHP = row.HP;
                data.m_Speed = row.Speed;
                data.m_CameraId = row.CameraId0;
                data.m_ProjectileEntityId = row.ProjectileEntityId;
                data.m_ProjectileType = row.ProjectileType;
            }
            else
            {
                Log.Warning("Can not find Player id '{0}'.", typeId);
            }
            data.OwnerCamp = ownerCamp;
            return data;
        }

        public override void Clear()
        {
            base.Clear();
            OwnerCamp = CampType.Unknown;
            m_MaxHP = 0;
            m_Speed = 0;
            m_CameraId = 0;
        }
        /// <summary>
        /// 最大生命。
        /// </summary>
        public float MaxHP
        {
            get
            {
                return m_MaxHP;
            }
        }
        /// <summary>
        /// 基础移速。
        /// </summary>
        public float Speed
        {
            get
            {
                return m_Speed;
            }
        }

        /// <summary>
        /// 相机编号。
        /// </summary>
        public int CameraId
        {
            get
            {
                return m_CameraId;
            }
        }
        public int ProjectileEntityId
        {
            get
            {
                return m_ProjectileEntityId;
            }
        }
        public string ProjectileType
        {
            get
            {
                return m_ProjectileType;
            }
        }
    }
}