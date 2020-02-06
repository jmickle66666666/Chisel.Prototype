using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Math/Int")]
public class IntMath : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	public VectorMathOperation operation;
	[Input] public int a;
	[Input] public int b;
	[Output] public int output;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		a = GetInputValue<int>("a", a);
		b = GetInputValue<int>("b", b);
		if (port.fieldName == "output") {
			switch (operation) {
				case VectorMathOperation.Add:
					return a + b;
				case VectorMathOperation.Subtract:
					return a - b;
				case VectorMathOperation.Multiply:
					return a * b;
				case VectorMathOperation.Divide:
					return a / b;
			}
		}
		return null; // Replace this
	}
}