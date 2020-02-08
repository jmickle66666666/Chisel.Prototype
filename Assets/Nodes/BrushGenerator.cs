using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chisel.Components;
using Chisel.Core;

[ExecuteInEditMode]
public class BrushGenerator : ChiselGeneratorComponent, ISerializationCallbackReceiver
{
    public GeneratorGraph generator;
    public BrushMesh[] brushMeshes;
    public ChiselBrushDefinition definition;

    public const string kNodeTypeName = "NodeGenerator";
    public override string NodeTypeName { get { return kNodeTypeName; } }
    
    protected override void UpdateGeneratorInternal()   {
        if (generator == null) {
            return;
        }

        if (definition == null) {
            definition = new ChiselBrushDefinition();
        }

        generator.ApplyInputProperties(this);
        brushMeshes = generator.GetOutput();
        brushContainerAsset.SetSubMeshes(brushMeshes);
        brushContainerAsset.SetDirty();
    }

    public override void UpdateGenerator()
    {
        generator.RefreshInputNodes();
        base.UpdateGenerator();
    }

    public Dictionary<string, Bounds> properties = new Dictionary<string, Bounds>();
    [HideInInspector] [SerializeField] List<string> propertyNames = new List<string>();
    [SerializeReference] [HideInInspector] [SerializeField] List<Bounds> propertyValues = new List<Bounds>();

    public void OnBeforeSerialize()
    {
        propertyNames.Clear();
        propertyValues.Clear();
        foreach (var item in properties)
        {
            propertyNames.Add(item.Key);
            propertyValues.Add(item.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        Debug.Assert(propertyNames.Count == propertyValues.Count);
        properties.Clear();
        for (int i = 0; i < propertyNames.Count; i++)
            properties[propertyNames[i]] = propertyValues[i];
        propertyNames.Clear();
        propertyValues.Clear();
    }
}
