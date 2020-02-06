using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Data/Vector2")]
public class Vector2Node : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	[Input] public Vector2 input;
	[Input] public float x;
	[Input] public float y;
	[Output] public Vector2 output;
	[Output] public Vector2Int outputInt;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {

		input = GetInputValue<Vector2>("input", input);
		input.x = GetInputValue<float>("x", input.x);
		input.y = GetInputValue<float>("y", input.y);
		
		if (port.fieldName == "output") {
			return input;
		}

		if (port.fieldName == "outputInt") {
			return new Vector2Int(
				Mathf.FloorToInt(input.x),
				Mathf.FloorToInt(input.y)
			);
		}
		
		return null; // Replace this
	}
}