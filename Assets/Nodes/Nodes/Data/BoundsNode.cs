using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Data/Bounds")]
public class BoundsNode : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	[Input] public Bounds bounds = new Bounds();
	[Input] public Vector3 size;
	[Input] public Vector3 center;
	[Input] public Vector3 min;
	[Input] public Vector3 max;
	[Output] public Bounds output;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {

		if (port.fieldName == "output") {
			bounds = GetInputValue<Bounds>("bounds", bounds);
			bounds.size = GetInputValue<Vector3>("size", bounds.size);
			bounds.center = GetInputValue<Vector3>("center", bounds.center);
			bounds.min = GetInputValue<Vector3>("min", bounds.min);
			bounds.max = GetInputValue<Vector3>("max", bounds.max);

			return bounds;
		}

		return null; // Replace this
	}
}