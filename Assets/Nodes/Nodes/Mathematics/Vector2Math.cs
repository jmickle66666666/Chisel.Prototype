using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

// TODO: move this out of here
public enum VectorMathOperation
{
	Add,
	Subtract,
	Multiply,
	Divide
}

[CreateNodeMenu("Math/Vector2")]
public class Vector2Math : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	public VectorMathOperation operation;
	[Input] public Vector2 input;
	[Input] public float value;
	[Output] public Vector2 output;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		if (port.fieldName == "output") {
			input = GetInputValue<Vector2>("input", input);
			value = GetInputValue<float>("value", value);
			switch (operation) {
				case VectorMathOperation.Add:
					return input + Vector2.one * value;
				case VectorMathOperation.Subtract:
					return input - Vector2.one * value;
				case VectorMathOperation.Multiply:
					return input * value;
				case VectorMathOperation.Divide:
					return input * value;
			}
		}
		return null; // Replace this
	}
}