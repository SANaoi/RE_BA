using UnityEngine;

namespace KSG
{
    public interface ILauncher
    {
        void Launch(EntityDataProjectile projectileData, Vector3 origin, Transform firingPoint);
    }
}