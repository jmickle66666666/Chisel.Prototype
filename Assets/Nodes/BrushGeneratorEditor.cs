using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chisel.Editors;
using UnityEditor;

[CustomEditor(typeof(BrushGenerator))]
[CanEditMultipleObjects]
public class BrushGeneratorEditor : ChiselGeneratorEditor<BrushGenerator>
{
    protected override void OnScene(SceneView sceneView, BrushGenerator generator) {
        // Debug.Log("BrushGeneratorEditor OnScene");
        (target as BrushGenerator).generator.OnScene(sceneView, generator);
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        DrawDefaultInspector();
    }
}
