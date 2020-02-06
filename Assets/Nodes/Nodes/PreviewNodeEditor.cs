using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNodeEditor;
using XNode;

[CustomNodeEditor(typeof(PreviewNode))]
public class PreviewNodeEditor : NodeEditor {
    private PreviewNode previewNode;

    Editor genericEditor;
    Object previewObject;

    public override void OnBodyGUI() {
        if (previewNode == null) previewNode = (PreviewNode) serializedObject.targetObject;
        base.OnBodyGUI();

        if (genericEditor == null) {
            
            if (previewObject != null) {
                if (previewObject.GetType() == typeof(Texture2D)) {
                    EditorGUI.DrawPreviewTexture(GUILayoutUtility.GetRect (200,200), (Texture) previewObject);
                }
            }

        } else {
            GUIStyle bgColor = new GUIStyle();
   
            bgColor.normal.background = EditorGUIUtility.whiteTexture;
            genericEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect (200,200), bgColor);
        }

        if (GUILayout.Button("Update")) {
            previewObject = previewNode.GetInputValue<Object>("previewObject", null);

            if (previewObject.GetType() == typeof(Texture2D)) {
                
            } else {
                UpdateGenericPreview();
            }

        }
    }

    void UpdateGenericPreview() {
        if (previewObject != null) {
            genericEditor = Editor.CreateEditor(previewObject);
        } else {
            genericEditor = null;
        }
    }
}