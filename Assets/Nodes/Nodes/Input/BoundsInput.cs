using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;

[CreateNodeMenu("Input/Bounds")]
public class BoundsInput : InputNode {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	public Bounds bounds = new Bounds(Vector3.zero, Vector3.one);
	[Output] public Bounds output;
	[Output] public Vector3 min;
	[Output] public Vector3 max;
	[Output] public Vector3 size;
	[Output] public Vector3 center;
	[Output] public Vector3 extents;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		if (port.fieldName == "output") return bounds;
		if (port.fieldName == "min") { return bounds.min; }
		if (port.fieldName == "max") { return bounds.max; }
		if (port.fieldName == "size") { return bounds.size; }
		if (port.fieldName == "center") { return bounds.center; }
		if (port.fieldName == "extents") { return bounds.extents; }
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