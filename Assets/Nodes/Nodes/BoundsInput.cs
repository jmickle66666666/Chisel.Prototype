using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BoundsInput : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	[Output] public Bounds bounds;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		if (port.fieldName == "bounds") return bounds;
		return null; // Replace this
	}
}