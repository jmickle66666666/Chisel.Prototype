using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Chisel.Core;

[CreateAssetMenu]
public class GeneratorGraph : NodeGraph { 
	public BrushMesh GetOutput()
    {
        for (int i = 0; i < nodes.Count; i++) {
            if (nodes[i] is BrushOutput) {
                var output = nodes[i] as BrushOutput;
                return output.GetInputValue<BrushMesh>("brushMesh", output.brushMesh);
            }
        }
        Debug.LogError("No valid output found for Generator Graph");
        return null;
    }
}