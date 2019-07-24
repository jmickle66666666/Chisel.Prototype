using ChiselHandles;
using UnityEditor;

enum ChiselToolState
{
    Create,
    Modify
}

class ChiselToolControl
{

    static IChiselTool currentTool;
    static IChiselHandle currentHandle;
    static ChiselToolState toolState;

    static void OnSceneGUI(SceneView sceneView)
    {
        switch (toolState) {
            case ChiselToolState.Create:
                CreateUpdate(sceneView);
                break;
            case ChiselToolState.Modify:
                ModifyUpdate(sceneView);
                break;
            default:
                // I hate switch statements
                break;
        }
    }

    static void CreateUpdate(SceneView sceneView)
    {
        if (currentHandle == null) {
            currentHandle = currentTool.OnCreate();
            if (currentHandle == null) { 
                OnCancelled();
                return;
            }
        }

        EditorGUI.BeginChangeCheck();
        var state = currentHandle.OnSceneGUI(sceneView);

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

    static void ModifyUpdate(SceneView sceneView)
    {
        currentHandle.OnSceneGUI(sceneView);
    }

    // Finished the creation step
    static void OnCreated()
    {

        currentHandle = currentTool.OnModify();
        toolState = ChiselToolState.Modify;
    }

    // Some validation failed
    static void OnCancelled()
    {
        // Go back to the start of the flow
        currentHandle = null;
        toolState = ChiselToolState.Create;

        // Handle Undo
        Undo.RevertAllInCurrentGroup();
        EditorGUIUtility.ExitGUI();
    }
    
    // This would be called by the shape edit mode, when an object is selected that has an associated IChiselTool
    static void OnSelect(IChiselTool tool)
    {
        toolState = ChiselToolState.Modify;
        currentTool = tool;
        currentHandle = currentTool.OnModify();
    }
    
}