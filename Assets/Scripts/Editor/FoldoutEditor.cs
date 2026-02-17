// Create a foldable menu that hides/shows the selected transform position.
// If no Transform is selected, the Foldout item will be folded until
// a transform is selected.

using UnityEditor;
using UnityEngine;

public class FoldoutEditor : EditorWindow
{
    bool showPosition = true;
    bool showPosition2 = true;
    string status = "Select a GameObject";
    string status2 = "Select a GameObject2";

    [MenuItem("Examples/Foldout Usage")]
    static void Init()
    {
        FoldoutEditor window = (FoldoutEditor)GetWindow(typeof(FoldoutEditor));
        window.Show();
    }

    public void OnGUI()
    {
        showPosition = EditorGUILayout.Foldout(showPosition, status);
        if (showPosition)
            if (Selection.activeTransform)
            {
                //Selection.activeTransform.position =
                //    EditorGUILayout.Vector3Field("Position", Selection.activeTransform.position);
                
                showPosition2 = EditorGUILayout.Foldout(showPosition2, status2);
                if (showPosition2)
                {
                    Selection.activeTransform.position =
                    EditorGUILayout.Vector3Field("Position", Selection.activeTransform.position);
                }

                status = Selection.activeTransform.name;
            }

        if (!Selection.activeTransform)
        {
            status = "Select a GameObject";
            showPosition = false;
        }
    }

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
}
