using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Math/Vector3")]
public class Vector3Math : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	public VectorMathOperation operation;
	[Input] public Vector3 input;
	[Input] public float value;
	[Input] public Vector3 otherVector;
	[Output] public Vector3 output;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {

		otherVector = GetInputValue<Vector3>("otherVector", otherVector);
		if (GetPort("value").IsConnected) {
			value = GetInputValue<float>("value", value);
			otherVector = Vector3.one * value;
		}

		if (port.fieldName == "output") {
			input = GetInputValue<Vector3>("input", input);
			value = GetInputValue<float>("value", value);
			switch (operation) {
				case VectorMathOperation.Add:
					return input + otherVector;
				case VectorMathOperation.Subtract:
					return input - otherVector;
				case VectorMathOperation.Multiply:
					return new Vector3(
						input.x * otherVector.x,
						input.y * otherVector.y,
						input.z * otherVector.z
					);
				case VectorMathOperation.Divide:
					return new Vector3(
						input.x / otherVector.x,
						input.y / otherVector.y,
						input.z / otherVector.z
					);
			}
		}
		return null; // Replace this
	}
}