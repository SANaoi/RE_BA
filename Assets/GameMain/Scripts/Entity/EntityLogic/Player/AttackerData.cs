using GameFramework;

namespace KSG
{
    public class AttackerData : IReference
    {
        public int AttackerId
        {
            get;
            private set;
        }
        public CampType CampType
        {
            get;
            private set;
        }
        public int ProjectileEntityId
        {
            get;
            private set;
        }

        public string ProjectileType
        {
            get;
            private set;
        }

        public AttackerData()
        {
            this.ProjectileType = null;
            this.ProjectileEntityId = -1;
            this.AttackerId = -1;
            this.CampType = CampType.Unknown;
        }
        public static AttackerData Create
        (
            int attackerId, 
            CampType campType,
            int projectileEntityId, 
            string projectileType)
        {
            AttackerData attackerData = ReferencePool.Acquire<AttackerData>();
            attackerData.AttackerId = attackerId;
            attackerData.CampType = campType;
            attackerData.ProjectileEntityId = projectileEntityId;
            attackerData.ProjectileType = projectileType;

            return attackerData;
        }
        public void Clear()
        {
            this.ProjectileEntityId = -1;
            this.ProjectileType = null;
        }
    }
}