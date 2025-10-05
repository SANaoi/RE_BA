// MultiImageController.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[RequireComponent(typeof(Image))]
public class MultiImageController : MonoBehaviour
{
    [Header("图片列表")]
    public List<Sprite> sprites = new List<Sprite>();

    [Header("当前显示索引")]
    [Range(0, 100)]
    [SerializeField] private int currentIndex; // 私有字段（动画会修改这个字段）
    private Animator animator;
    // 公开属性，用于监听变化
    public int CurrentIndex
    {
        get => currentIndex;
        set
        {
            if (currentIndex != value) // 仅当值变化时执行
            {
                currentIndex = value;
                UpdateDisplayImage(); // 触发图片更新
            }
        }
    }

    private Image image;

    // 初始化Image组件引用（编辑模式也会执行）
    private void OnEnable()
    {
        image = GetComponent<Image>();
        
        UpdateDisplayImage();
    }

    // 编辑模式下属性变化时自动调用（关键！）
    private void OnValidate()
    {
        // 确保获取到Image组件
        if (image == null)
            image = GetComponent<Image>();

        // 实时更新图片（编辑模式下立即生效）
        UpdateDisplayImage();
    }

    // 更新显示图片
    public void UpdateDisplayImage()
    {
        if (image == null) return;
        if (animator != null)
        {
            animator.enabled = true;
        }

        if (sprites.Count == 0)
        {
            if (image.sprite != null) // 仅在状态变化时才修改
            {
                image.sprite = null;
                MarkAsDirtyIfNeeded();
            }
            return;
        }

        currentIndex = Mathf.Clamp(currentIndex, 0, sprites.Count - 1);
        Sprite targetSprite = sprites[currentIndex];

        if (image.sprite != targetSprite) // 仅在图片变化时才更新
        {
            image.sprite = targetSprite;
            MarkAsDirtyIfNeeded();
        }
    }
    // 仅在必要时标记为脏（避免过度调用）
    private void MarkAsDirtyIfNeeded()
    {
#if UNITY_EDITOR
        // 运行时不标记（避免Play模式下污染场景）
        if (!Application.isPlaying)
        {
            // 仅当图片实际变化时才标记
            UnityEditor.EditorUtility.SetDirty(image);
            // UnityEditor.EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }
#endif
    }
}