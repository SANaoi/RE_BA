using UnityEngine;

namespace KSG
{
	public class CharacterData : EntityData
	{
		[SerializeField]
        private int m_OwnerId = 0;

        [SerializeField]
        private CampType m_OwnerCamp = CampType.Unknown;

		public CharacterData(int entityId, int typeId, CampType ownerCamp) : base(entityId, typeId)
		{
            m_OwnerCamp = ownerCamp;
		}

        /// <summary>
        /// 拥有者编号。
        /// </summary>
        public int OwnerId
        {
            get
            {
                return m_OwnerId;
            }
        }

        /// <summary>
        /// 拥有者阵营。
        /// </summary>
        public CampType OwnerCamp
        {
            get
            {
                return m_OwnerCamp;
            }
        }
	}
}