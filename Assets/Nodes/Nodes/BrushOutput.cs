using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Chisel.Core;

public class BrushOutput : Node {

	// TODO: Handle all brushes from just one input if possible
	[Input] public BrushMesh brushMesh;
	[Input] public BrushMesh[] brushMeshArrays;

	public BrushMesh[] GetBrushes()
	{
		List<BrushMesh> output = new List<BrushMesh>();
		var brushes = GetInputValues<BrushMesh>("brushMesh", brushMesh);
		foreach (var brush in brushes) {
			if (brush != null) {
				output.Add(brush);
			}
		}

		var brushArrays = GetInputValues<BrushMesh[]>("brushMeshArrays", brushMeshArrays);
		if (brushArrays == null) {
			goto JustOutput;
		}

		foreach (var brushArray in brushArrays) {
			if (brushArray != null) {
				output.AddRange(brushArray);
			}
		}

		JustOutput:


		return output.ToArray();
	}
}