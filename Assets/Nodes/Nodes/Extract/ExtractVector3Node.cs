using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Extract/Vector3")]
public class ExtractVector3Node : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	[Input] public Vector3 vector3;
	[Output] public float x;
	[Output] public float y;
	[Output] public float z;
	[Output] public float magnitude;
	[Output] public Vector3 normalized;
	[Output] public float sqrMagnitude;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		vector3 = GetInputValue<Vector3>("vector3", vector3);
		if (port.fieldName == "x") { return vector3.x; }
		if (port.fieldName == "y") { return vector3.y; }
		if (port.fieldName == "z") { return vector3.z; }
		if (port.fieldName == "magnitude") { return vector3.magnitude; }
		if (port.fieldName == "normalized") { return vector3.normalized; }
		if (port.fieldName == "sqrMagnitude") { return vector3.sqrMagnitude; }
		return null; // Replace this
	}
}