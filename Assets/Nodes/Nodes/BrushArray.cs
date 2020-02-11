using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Chisel.Core;

public class BrushArray : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	[Input] public BrushMesh input;
	[Input] public Vector3 offset;
	[Input] public Vector3 scale;
	[Input] public Vector3 rotation;
	[Input] public int count;
	[Output] public BrushMesh[] output; 

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		
		if (port.fieldName == "output") {
			input = GetInputValue<BrushMesh>("input", input);
			offset = GetInputValue<Vector3>("offset", offset);
			scale = GetInputValue<Vector3>("scale", scale);
			rotation = GetInputValue<Vector3>("rotation", rotation);
			count = GetInputValue<int>("count", count);

			

			output = new BrushMesh[count];
			for (int i = 0; i < count; i++) {
				output[i] = new BrushMesh(input);
				var verts = output[i].vertices;
				for (int j = 0; j < verts.Length; j++) {

					Matrix4x4 matrix = Matrix4x4.TRS(
						offset * i,
						Quaternion.Euler(rotation * i),
						Vector3.one + scale * i
					);

					verts[j] = matrix.MultiplyPoint3x4(verts[j]);
				}
				output[i].vertices = verts;
			}

			return output;
		}

		return null; // Replace this
	}
}