// MultiImageEditor.cs
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MultiImageController))]
public class MultiImageEditor : Editor
{
    private MultiImageController targetController;

    private void OnEnable()
    {
        targetController = (MultiImageController)target;
    }

    public override void OnInspectorGUI()
    {
        // 记录修改前的索引（用于检测变化）
        int oldIndex = targetController.CurrentIndex;

        // 绘制默认属性面板（包括sprites列表和currentIndex）
        DrawDefaultInspector();

        // 检测索引是否被修改（编辑时手动修改滑块或输入框）
        if (targetController.CurrentIndex != oldIndex || 
            GUILayout.Button("刷新图片显示")) // 手动刷新按钮（可选）
        {
            // 实时更新图片显示（编辑模式下生效）
            targetController.UpdateDisplayImage();

            // 标记场景为已修改（确保编辑时的变化被保存）
            EditorUtility.SetDirty(targetController.gameObject);
        }
    }
}