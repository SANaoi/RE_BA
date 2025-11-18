using UnityEngine;
using UnityGameFramework.Runtime;

namespace KSG
{
    public class CrosshairForm : UGuiForm
    {

        [SerializeField] private RectTransform crosshairTransform;

        private float currentSize;
        public float targetSize
        { 
            get { return _targetSize; } 
            set { _targetSize = value; } 
        }
        private float _targetSize;
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (currentSize != targetSize)
            {
                if (targetSize >= PlayerConstantData.CrosshairData.MAXSIZE)
                {
                    targetSize = PlayerConstantData.CrosshairData.MAXSIZE;
                }
                currentSize = Mathf.Lerp(currentSize, targetSize, Time.deltaTime * PlayerConstantData.CrosshairData.SPEED);
                if (Mathf.Abs(currentSize - targetSize) < 0.01f)
                {
                    currentSize = targetSize;
                }
                Log.Debug(currentSize);
                crosshairTransform.sizeDelta = new Vector2(currentSize, currentSize);
            }
        }
    }
}