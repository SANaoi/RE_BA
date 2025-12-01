using GameFramework;

namespace KSG
{
    public class EntityDataProjectile : EntityData
    {
        /// <summary>
        /// 获取伤害。
        /// </summary>
        public float Damage
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取射速。
        /// </summary>
        public float Speed
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取溅射伤害。
        /// </summary>
        public float SplashDamage
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取溅射范围。
        /// </summary>
        public float SplashRange
        {
            get;
            private set;
        }
    }
}