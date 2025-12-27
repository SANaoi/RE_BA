using Cinemachine;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace KSG
{
    public class CameraLogic : EntityLogicBase
    {
        public Transform target;

        EntityDataCamera cameraData;
        private UnityEngine.Camera m_Camera;
        public CinemachineVirtualCamera virtualCamera;
        CinemachineFramingTransposer framingTransposer;
        public InputProviderControl inputProvider;
        float targetDistance;
        private bool isDistanceDirty = true;
        [SerializeField][Range(0f, 10f)] private float defaultDistance = 2f;
        [SerializeField][Range(0f, 10f)] private float minimumDistance = 1f;
        [SerializeField][Range(0f, 10f)] private float maximumDistance = 3f;
        [SerializeField][Range(0f, 10f)] private float smoothing = 4f;
        [SerializeField][Range(0f, 10f)] private float zoomSensitivity = 0.5f;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            cameraData = userData as EntityDataCamera;
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
            framingTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
            inputProvider = GetComponent<InputProviderControl>();

            targetDistance = defaultDistance;
            m_Camera = UnityEngine.Camera.main;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            ScrollWheel();
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
            virtualCamera.Follow = target;
            virtualCamera.LookAt = target;
            target.position = cameraData.DefaultLocalPosition;
        }
        void ScrollWheel()
        {
            var scrollValue = inputProvider.GetAxisValue(2) * zoomSensitivity;
            float currentDistance = framingTransposer.m_CameraDistance;

            // 计算新目标距离
            float newTarget = Mathf.Clamp(targetDistance + scrollValue, minimumDistance, maximumDistance);
            if (newTarget != targetDistance)
            {
                targetDistance = newTarget;
                isDistanceDirty = true; // 标记为脏数据（有变化）
            }

            if (!isDistanceDirty) return;

            if (currentDistance == targetDistance)
            {
                return;
            }

            float lerpedZoomValue = Mathf.Lerp(currentDistance, targetDistance, smoothing * Time.deltaTime);

            framingTransposer.m_CameraDistance = lerpedZoomValue;
        }

        public void SetTargetPosition(Vector3 localPosition)
        {
            if (target == null)
            {
                Log.Error("Camera target is null");
                return;
            }
            if (localPosition == target.position) return;
            target.localPosition = localPosition;
        }
        
    }
}