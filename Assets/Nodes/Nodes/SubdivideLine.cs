using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class SubdivideLine : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	[Input] public Line line;
	public bool bySubdivisions;
	public bool useMidpoints;
	[Input] public int subdivisions;
	[Input] public float maxDistance;
	[Output] public Vector3[] points;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {

		if (port.fieldName == "points") {

			line = GetInputValue<Line>("line", line);
			if (bySubdivisions) {
				subdivisions = GetInputValue<int>("subdivisions", subdivisions);
			} else {
				maxDistance = GetInputValue<float>("maxDistance", maxDistance);
				float length = Vector3.Distance(line.start, line.end);
				subdivisions = Mathf.RoundToInt(length / Mathf.Max(maxDistance,1));
			}

			Vector3 offset = line.end - line.start;
			Vector3 diff = offset / subdivisions;
			Vector3 halfDiff = diff/2;
			Vector3[] output = new Vector3[useMidpoints?subdivisions:subdivisions + 1];
			for (int i = 0; i < subdivisions; i++) {
				if (useMidpoints) {
					output[i] = line.start + (diff * i) + halfDiff;
				} else {
					output[i] = line.start + (diff * i);
				}
			}
			if (!useMidpoints) output [subdivisions] = line.end;
			return output;

		}

		return null; // Replace this
	}
}