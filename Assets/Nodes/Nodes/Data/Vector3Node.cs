using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Data/Vector3")]
public class Vector3Node : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	[Input] public Vector3 input;
	[Input] public float x;
	[Input] public float y;
	[Input] public float z;
	[Output] public Vector3 output;
	[Output] public Vector3Int outputInt;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {

		input = GetInputValue<Vector3>("input", new Vector3());
		input.x = GetInputValue<float>("x", input.x);
		input.y = GetInputValue<float>("y", input.y);
		input.z = GetInputValue<float>("z", input.z);

		if (port.fieldName == "output") {
			return input;
		}

		if (port.fieldName == "outputInt") {
			return new Vector3Int(
				Mathf.FloorToInt(input.x),
				Mathf.FloorToInt(input.y),
				Mathf.FloorToInt(input.z)
			);
		}

		return null; // Replace this
	}
}