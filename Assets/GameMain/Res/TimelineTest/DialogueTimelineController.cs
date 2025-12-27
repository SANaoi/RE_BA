using UnityEngine;
using System.Collections.Generic;

namespace KSG.Editor
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DialogueTimelineController : MonoBehaviour
    {
        [Header("Sprite 资源列表")]
        [Tooltip("存储所有可用的Sprite图片")]
        public List<Sprite> spriteList = new List<Sprite>();

        private SpriteRenderer spriteRenderer;

        // 当前使用的Sprite索引
        private int currentSpriteIndex = -1;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// 通过索引设置当前显示的Sprite
        /// </summary>
        /// <param name="index">Sprite在列表中的索引</param>
        public void SetSpriteByIndex(int index)
        {
            if (index >= 0 && index < spriteList.Count)
            {
                currentSpriteIndex = index;
                spriteRenderer.sprite = spriteList[index];
            }
            else
            {
                Debug.LogWarning($"Sprite索引 {index} 超出范围！");
            }
        }

        /// <summary>
        /// 通过名称设置当前显示的Sprite
        /// </summary>
        /// <param name="spriteName">Sprite的名称</param>
        public void SetSpriteByName(string spriteName)
        {
            for (int i = 0; i < spriteList.Count; i++)
            {
                if (spriteList[i] != null && spriteList[i].name == spriteName)
                {
                    currentSpriteIndex = i;
                    spriteRenderer.sprite = spriteList[i];
                    return;
                }
            }

            Debug.LogWarning($"未找到名称为 {spriteName} 的Sprite！");
        }

        /// <summary>
        /// 设置Sprite的位置
        /// </summary>
        public void SetPosition(Vector3 position)
        {
            transform.localPosition = position;
        }

        /// <summary>
        /// 设置Sprite的旋转角度
        /// </summary>
        public void SetRotation(Vector3 eulerAngles)
        {
            transform.localEulerAngles = eulerAngles;
        }

        /// <summary>
        /// 设置Sprite的缩放
        /// </summary>
        public void SetScale(Vector3 scale)
        {
            transform.localScale = scale;
        }

        /// <summary>
        /// 设置Sprite的颜色
        /// </summary>
        public void SetColor(Color color)
        {
            spriteRenderer.color = color;
        }

        /// <summary>
        /// 设置Sprite的透明度
        /// </summary>
        public void SetAlpha(float alpha)
        {
            Color currentColor = spriteRenderer.color;
            currentColor.a = Mathf.Clamp01(alpha);
            spriteRenderer.color = currentColor;
        }

        /// <summary>
        /// 获取当前使用的Sprite索引
        /// </summary>
        public int GetCurrentSpriteIndex()
        {
            return currentSpriteIndex;
        }

        /// <summary>
        /// 在Inspector中快速预览指定索引的Sprite
        /// </summary>
        [ContextMenu("预览第一个Sprite")]
        private void PreviewFirstSprite()
        {
            if (spriteList.Count > 0)
            {
                SetSpriteByIndex(0);
            }
        }
    }
}