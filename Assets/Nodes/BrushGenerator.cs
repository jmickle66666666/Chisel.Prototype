using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chisel.Components;
using Chisel.Core;

[ExecuteInEditMode]
public class BrushGenerator : ChiselGeneratorComponent
{
    public GeneratorGraph generator;
    public BrushMesh[] brushMeshes;
    public ChiselBrushDefinition definition;

    public const string kNodeTypeName = "NodeGenerator";
    public override string NodeTypeName { get { return kNodeTypeName; } }

    public Dictionary<string, object> properties = new Dictionary<string, object>();
    GeneratorGraph graphInstance;

    protected override void UpdateGeneratorInternal()   {
        if (generator == null) {
            return;
        }

        if (graphInstance == null) {
            graphInstance = generator.Copy() as GeneratorGraph;
        }
        
        if (definition == null) {
            definition = new ChiselBrushDefinition();
        }

        graphInstance.ApplyInputProperties(this);

        brushMeshes = graphInstance.GetOutput();
        brushContainerAsset.SetSubMeshes(brushMeshes);
        brushContainerAsset.SetDirty();
    }

    public override void UpdateGenerator()
    {
        generator.RefreshInputNodes();
        base.UpdateGenerator();
    }
}
