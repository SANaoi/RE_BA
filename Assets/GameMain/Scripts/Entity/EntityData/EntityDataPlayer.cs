using GameFramework.DataTable;
using UnityEngine;

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
        public EntityDataPlayer(int entityId, int typeId)
            : base(entityId, typeId, CampType.Player)
        {
            IDataTable<DRPlayer> dtPlayer = GameEntry.DataTable.GetDataTable<DRPlayer>();
            DRPlayer drPlayer = dtPlayer.GetDataRow(TypeId);
            if (drPlayer == null)
            {
                return;
            }

            m_MaxHP = drPlayer.HP;
            m_Speed = drPlayer.Speed;
            m_CameraId = drPlayer.CameraId0;

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
    }
}