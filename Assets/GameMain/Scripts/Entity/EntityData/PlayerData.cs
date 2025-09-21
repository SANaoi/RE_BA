using GameFramework.DataTable;
using UnityEngine;

namespace KSG
{
	public class PlayerData : CharacterData
	{
        [SerializeField]
        private float m_MaxHP = 0;
		public PlayerData(int entityId, int typeId)
            : base(entityId, typeId, CampType.Player)
		{
            IDataTable<DRPlayer> dtPlayer = GameEntry.DataTable.GetDataTable<DRPlayer>();
            DRPlayer drPlayer = dtPlayer.GetDataRow(TypeId);
            if (drPlayer == null)
            {
                return;
            }

            m_MaxHP = drPlayer.HP;

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
	}
}