using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Data/Int")]
public class IntNode : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	[Input] public int value;
	[Output] public int outValue;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		if (port.fieldName == "outValue") {
			value = GetInputValue<int>("value", value);
			return value;
		}
		return null; // Replace this
	}
}