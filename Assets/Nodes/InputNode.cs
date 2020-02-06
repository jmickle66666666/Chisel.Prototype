using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;

public class InputNode : Node
{
    public string inputName;
    public virtual void OnScene(SceneView sceneView, BrushGenerator generator) {

    }

    public virtual void UpdateProperties(BrushGenerator generator) {
        
    }
}
