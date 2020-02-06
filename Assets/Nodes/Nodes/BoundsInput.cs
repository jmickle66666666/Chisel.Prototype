using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;

public class BoundsInput : InputNode {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	public Bounds bounds;
	[Output] public Bounds output;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		if (port.fieldName == "output") return bounds;
		return null; // Replace this
	}

	public override void OnScene(SceneView sceneView, BrushGenerator generator) {
		// Debug.Log("Bounds Input OnScene");
		EditorGUI.BeginChangeCheck();

		Bounds newBounds;
		if (generator.properties.ContainsKey(inputName)) {
			newBounds = (Bounds)generator.properties[inputName];
		} else {
			newBounds = bounds;
			generator.properties.Add(inputName, bounds);
		}

		newBounds = UnitySceneExtensions.SceneHandles.BoundsHandle(newBounds, Quaternion.identity);

		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(generator, "Modified " + generator.NodeTypeName);
			generator.properties[inputName] = newBounds;
			generator.UpdateGenerator();
		}
	}

	public override void UpdateProperties(BrushGenerator generator)
	{
		if (generator.properties.ContainsKey(inputName)) {
			bounds = (Bounds) generator.properties[inputName];
		}
	}
}