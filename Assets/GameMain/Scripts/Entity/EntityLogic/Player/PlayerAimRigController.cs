using UnityEngine;

namespace KSG
{
    public class PlayerAimRigController : MonoBehaviour
    {

        [Range(-180, 180f)]
        public float x;

        [Range(-180, 180f)]
        public float y;

        [Range(-180, 180f)]
        public float z;
        private Vector3 currentVelocity;
        private void Update()
        {
            transform.position = Vector3.SmoothDamp
            (
                transform.position,
                Camera.main.transform.position + Camera.main.transform.rotation * new Vector3(x, y, z),
                ref currentVelocity,
                5 * Time.deltaTime
            );
        }
    }
}