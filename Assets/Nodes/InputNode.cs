using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;

[NodeTint(.6f,.5f,0f)]
public class InputNode : Node
{
    public string inputName;
    public virtual void OnScene(SceneView sceneView, BrushGenerator generator) {

    }

    public virtual void UpdateProperties(BrushGenerator generator) {

    }
}
