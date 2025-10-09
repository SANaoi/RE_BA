using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor;

[ExecuteInEditMode]
public class AnimatedImageSwitcher : MonoBehaviour
{
    [System.Serializable]
    public class ImageGroup
    {
        public string groupName;
        public List<Sprite> sprites = new List<Sprite>();
    }

    [Header("组件引用")]
    public Image targetImage;

    [Header("图片组设置")]
    public List<ImageGroup> imageGroups = new List<ImageGroup>();

    [Header("当前设置")]
    public int currentGroupIndex;
    public int currentSpriteIndex;

    private void OnEnable()
    {
        // 确保在编辑模式下也能更新
        if (!Application.isPlaying)
        {
            UpdateImage();
        }
    }

    private void Update()
    {
        // 编辑模式下实时更新
        if (!Application.isPlaying)
        {
            UpdateImage();
        }
        UpdateImage();
    }

    /// <summary>
    /// 更新图片显示
    /// </summary>
    public void UpdateImage()
    {
        if (targetImage == null)
        {
            // 尝试自动获取组件
            targetImage = GetComponent<Image>();
            return;
        }

        // 检查索引有效性
        if (currentGroupIndex >= 0 && currentGroupIndex < imageGroups.Count)
        {
            var currentGroup = imageGroups[currentGroupIndex];
            
            if (currentSpriteIndex >= 0 && currentSpriteIndex < currentGroup.sprites.Count)
            {
                targetImage.sprite = currentGroup.sprites[currentSpriteIndex];
            }
        }
    }

    /// <summary>
    /// 通过动画事件调用切换图片
    /// </summary>
    /// <param name="groupIndex">组索引</param>
    /// <param name="spriteIndex">图片索引</param>
    public void SetSprite(int groupIndex, int spriteIndex)
    {
        currentGroupIndex = groupIndex;
        currentSpriteIndex = spriteIndex;
        UpdateImage();
    }

    /// <summary>
    /// 通过组名和图片索引切换图片
    /// </summary>
    public void SetSprite(string groupName, int spriteIndex)
    {
        int groupIndex = imageGroups.FindIndex(g => g.groupName == groupName);
        if (groupIndex != -1)
        {
            currentGroupIndex = groupIndex;
            currentSpriteIndex = spriteIndex;
            UpdateImage();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AnimatedImageSwitcher))]
public class AnimatedImageSwitcherEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AnimatedImageSwitcher switcher = (AnimatedImageSwitcher)target;

        // 添加手动更新按钮
        if (GUILayout.Button("更新图片显示"))
        {
            switcher.UpdateImage();
        }

        // 防止索引越界
        if (switcher.imageGroups.Count > 0)
        {
            switcher.currentGroupIndex = Mathf.Clamp(switcher.currentGroupIndex, 0, switcher.imageGroups.Count - 1);
            
            var currentGroup = switcher.imageGroups[switcher.currentGroupIndex];
            if (currentGroup.sprites.Count > 0)
            {
                switcher.currentSpriteIndex = Mathf.Clamp(switcher.currentSpriteIndex, 0, currentGroup.sprites.Count - 1);
            }
        }
    }
}
#endif