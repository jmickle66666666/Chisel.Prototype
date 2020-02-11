using System.Collections;
using System.Collections.Generic;
using XNode;
using UnityEngine;
using UnityEditor;
using Chisel.Core;

[CreateAssetMenu]
public class GeneratorGraph : NodeGraph { 

	public BrushMesh[] GetOutput()
    {
        for (int i = 0; i < nodes.Count; i++) {
            if (nodes[i] is BrushOutput) {
                var output = nodes[i] as BrushOutput;
                
                return output.GetBrushes();
            }
        }
        Debug.LogError("No valid output found for Generator Graph");
        return null;
    }

    public List<InputNode> inputNodes;

    public void RefreshInputNodes()
    {
        // TODO: Detect when a graph is modified and only call this then
        if (inputNodes == null) inputNodes = new List<InputNode>();
        inputNodes.Clear();
        for (int i = 0; i < nodes.Count; i++) {
            if (nodes[i] is InputNode) {
                inputNodes.Add(nodes[i] as InputNode);
            }
        }
    }

    public void OnScene(SceneView sceneView, BrushGenerator generator)
    {
        // Debug.Log($"Generator Graph OnScene, InputNode Count: {inputNodes.Count}");
        RefreshInputNodes();
        for (int i = 0; i < inputNodes.Count; i++) {
            inputNodes[i].OnScene(sceneView, generator);
        }
    }

    public void ApplyInputProperties(BrushGenerator generator)
    {
        RefreshInputNodes();
        for (int i = 0; i < inputNodes.Count; i++) {
            inputNodes[i].UpdateProperties(generator);
        }
    }
}