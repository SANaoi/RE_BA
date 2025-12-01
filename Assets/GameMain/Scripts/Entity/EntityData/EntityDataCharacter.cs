using GameFramework;
using UnityEngine;

namespace KSG
{
    public abstract class EntityDataCharacter : EntityData
    {

        [SerializeField]
        private CampType m_OwnerCamp = CampType.Unknown;

        /// <summary>
        /// 拥有者阵营。
        /// </summary>
        public CampType OwnerCamp
        {
            get
            {
                return m_OwnerCamp;
            }
            set
            {
                m_OwnerCamp = value;
            }
        }
    }
}