using ChiselHandles;
using UnityEditor;
using UnityEngine;
using Chisel.Editors;

enum ChiselToolState
{
    Create,
    Modify
}

class ChiselToolControl : ChiselGeneratorToolMode
{
    public override string ToolName { get { return "Too."; } }
    // public bool ShowCompleteOutline { get { return false; } }
    // public bool CanSelectSurfaces { get { return false; } }
    // public bool EnableComponentEditors { get { return false; } }
    public override void OnDisable() {
        currentTool = null;
        toolState = ChiselToolState.Create;
        currentHandle = null;
    }
    public override void OnEnable() {
        currentTool = new BoxTool();
        controlID = GUIUtility.GetControlID($"ToolControl {currentTool.name}".GetHashCode(), FocusType.Keyboard);
        toolState = ChiselToolState.Create;
        currentHandle = null;
    }

    int controlID;

    IChiselTool currentTool;
    IChiselHandle currentHandle;
    ChiselToolState toolState;

    public override void OnSceneGUI(SceneView sceneView, Rect dragArea)
    {
        base.OnSceneGUI(sceneView, dragArea);

        switch (toolState) {
            case ChiselToolState.Create:
                CreateUpdate(sceneView, dragArea);
                break;
            case ChiselToolState.Modify:
                ModifyUpdate(sceneView, dragArea);
                break;
            default:
                // I hate switch statements
                break;
        }
    }

    void CreateUpdate(SceneView sceneView, Rect dragArea)
    {
        if (currentHandle == null) {
            currentHandle = currentTool.OnCreate();
            if (currentHandle == null) { 
                OnCancelled();
                return;
            }
        }

        EditorGUI.BeginChangeCheck();
        var state = currentHandle.OnSceneGUI(sceneView, dragArea);

        if (EditorGUI.EndChangeCheck())
        {
            // handle Undo here somehow
            currentTool.OnBuild();
        }

        switch (state) {
            case ChiselHandleState.Processing:
                break;
            case ChiselHandleState.Finished:
                OnCreated();
                break;
            case ChiselHandleState.Cancelled:
            default:
                OnCancelled();
                break;
        }
    }

    void ModifyUpdate(SceneView sceneView, Rect dragArea)
    {
        currentHandle.OnSceneGUI(sceneView, dragArea);
    }

    // Finished the creation step
    void OnCreated()
    {
        currentHandle = currentHandle.Next();

        if (currentHandle == null) {
            currentHandle = currentTool.OnModify();
            toolState = ChiselToolState.Modify;
        }
    }

    // Some validation failed
    void OnCancelled()
    {
        // Go back to the start of the flow
        currentHandle = null;
        toolState = ChiselToolState.Create;

        // Handle Undo
        Undo.RevertAllInCurrentGroup();
        EditorGUIUtility.ExitGUI();
    }
    
    // This would be called by the shape edit mode, when an object is selected that has an associated IChiselTool
    void OnSelect(IChiselTool tool)
    {
        toolState = ChiselToolState.Modify;
        currentTool = tool;
        currentHandle = currentTool.OnModify();
    }
    
}