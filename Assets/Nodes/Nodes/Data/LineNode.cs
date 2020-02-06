using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

// Create a Line object from two Vector3s
[CreateNodeMenu("Data/Line")]
public class LineNode : Node {

	// Input Output attributes 
	[Input] public Vector3 start;
	[Input] public Vector3 end;
	[Output] public Line line;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {

		if (port.fieldName == "line") {

			// Recalculate inputs if they are connected, else keep the existing value.
			start = GetInputValue<Vector3>("start", start);
										// ^        ^- Default value
										// '- property name


			end = GetInputValue<Vector3>("end", end);

			// Return the calculate output value
			return new Line(start, end);
		}

		return null;
	}
}