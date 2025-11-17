using UnityEngine;
using UnityGameFramework.Runtime;

namespace KSG
{
    public class CrosshairForm : UGuiForm
    {
        private Vector3[] points;
        private float lastSize;

        [SerializeField] protected LineRenderer lineRenderer;
        [SerializeField] protected float crosshairSize = 0.05f; // 准星大小
        [SerializeField] protected Color crosshairColor = Color.red; // 准星颜色

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            if (lineRenderer == null)
            {
                lineRenderer = GetComponent<LineRenderer>();
            }
            // 初始化线段属性
            lineRenderer.positionCount = 8; // 4条线段，每条2个点
            lineRenderer.startColor = crosshairColor;
            lineRenderer.endColor = crosshairColor;
            lineRenderer.startWidth = 0.01f;
            lineRenderer.endWidth = 0.01f;

            // 将准星定位到屏幕中心（需作为相机子物体，或设置世界坐标）
            transform.SetParent(Camera.main.transform, false); 
            transform.localPosition = new Vector3(0, 0, 2f); // 距离相机2米处
            transform.localRotation = Quaternion.identity;

            points = new Vector3[8];
        }
        void UpdateCrosshairShape()
        {
            points[0] = new Vector3(0, crosshairSize, 0);
            points[1] = new Vector3(0, crosshairSize * 0.6f, 0);
            points[2] = new Vector3(0, -crosshairSize, 0);
            points[3] = new Vector3(0, 0, 0);
            points[4] = new Vector3(-crosshairSize, 0, 0);
            points[5] = new Vector3(-crosshairSize * 0.6f, 0, 0);
            points[6] = new Vector3(crosshairSize, 0, 0);
            points[7] = new Vector3(crosshairSize * 0.6f, 0, 0);

            lineRenderer.SetPositions(points);
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (crosshairSize != lastSize)
            {
                UpdateCrosshairShape();
                lastSize = crosshairSize;
            }
        }
    }
}