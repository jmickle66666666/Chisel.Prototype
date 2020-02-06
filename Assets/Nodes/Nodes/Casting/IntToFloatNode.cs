using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Cast/Int to Float")]
public class IntToFloatNode : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	[Input] public int input;
	[Output] public float output;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		input = GetInputValue<int>("input", input);
		if (port.fieldName == "output") {
			return (float) input;
		}
		return null; // Replace this
	}
}