using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class TransformationNode : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	[Input] public Vector3[] points;
	[Input] public Mesh mesh = null;
	[Input] public Vector3 translation = Vector3.zero;
	[Input] public Quaternion rotation = Quaternion.identity;
	[Input] public Vector3 scale = Vector3.one;
	[Output] public Vector3[] pointOutput;
	[Output] public Mesh meshOutput;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		var matrix = Matrix4x4.TRS(
			GetInputValue<Vector3>("translation", translation),
			GetInputValue<Quaternion>("rotation", rotation),
			GetInputValue<Vector3>("scale", scale)
		);

		points = GetInputValue<Vector3[]>("points", points);
		mesh = GetInputValue<Mesh>("mesh", mesh);
		if (mesh != null) {
			points = mesh.vertices;

		}

		points = TransformPoints(points, matrix);

		if (port.fieldName == "pointOutput") {
			return points;
		}

		if (port.fieldName == "meshOutput") {
			mesh.vertices = points;
			mesh.RecalculateBounds();
			return mesh;
		}

		return null; // Replace this
	}

	public Vector3[] TransformPoints(Vector3[] inputPoints, Matrix4x4 matrix)
	{
		Vector3[] output = new Vector3[inputPoints.Length];
		for (int i = 0; i < inputPoints.Length; i++) {
			output[i] = matrix.MultiplyPoint3x4(inputPoints[i]);
		}
		return output;
	}
}