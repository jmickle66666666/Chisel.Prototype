using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Extract/Mesh")]
public class ExtractMeshData : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	[Input] public Mesh mesh;
	[Output] public Vector3[] vertices;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		if (port.fieldName == "vertices") {
			Mesh mesh = GetInputValue<Mesh>("mesh", null);
			if (mesh != null) {
				return mesh.vertices;
			}
		}
		return null; // Replace this
	}
}