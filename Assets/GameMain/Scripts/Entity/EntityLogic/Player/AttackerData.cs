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
        public float Spread
        {
            get;
            private set;
        }

        public float FireRate
        {
            get;
            private set;
        }
        public bool IsMultiAttack
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
            this.Spread = 0;
            this.FireRate = 0;
            this.IsMultiAttack = false;
            this.ProjectileType = null;
            this.ProjectileEntityId = -1;
            this.AttackerId = -1;
            this.CampType = CampType.Unknown;
        }
        public static AttackerData Create
        (
            float spread, 
            float fireRate, 
            bool isMultiAttack, 
            int projectileEntityId, 
            string projectileType, 
            int attackerId, 
            CampType campType)
        {
            AttackerData attackerData = ReferencePool.Acquire<AttackerData>();
            attackerData.Spread = spread;
            attackerData.FireRate = fireRate;
            attackerData.IsMultiAttack = isMultiAttack;
            attackerData.AttackerId = attackerId;
            attackerData.CampType = campType;
            attackerData.ProjectileEntityId = projectileEntityId;
            attackerData.ProjectileType = projectileType;

            return attackerData;
        }
        public void Clear()
        {
            this.Spread = 0f;
            this.FireRate = 0f;
            this.IsMultiAttack = false;
            this.ProjectileEntityId = -1;
            this.ProjectileType = null;
        }
    }
}