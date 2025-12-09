using UnityEngine;

namespace KSG
{
    public abstract class Launcher : MonoBehaviour, ILauncher
    {
        public virtual void Launch(EntityDataProjectile projectileData, Vector3 origin, Transform firingPoint)
        {
            
        }
        public virtual void Launch(AttackerData attackerData, Vector3 origin, Transform firingPoint)
        {
            
        }
        public virtual void Launch(EntityDataProjectile projectileData, Vector3 origin, Transform[] firingPoints)
        {
            
        }
    }
}