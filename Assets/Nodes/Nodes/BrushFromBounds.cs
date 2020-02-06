using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Chisel.Core;

public class BrushFromBounds : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	[Input] public Bounds bounds;
	[Output] public BrushMesh brush;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		if (port.fieldName == "brush") {

			bounds = GetInputValue<Bounds>("bounds", bounds);

			var surface = new ChiselSurface
			{
				surfaceDescription  = SurfaceDescription.Default,
				brushMaterial = ChiselBrushMaterial.CreateInstance(
					ChiselMaterialManager.DefaultWallMaterial, 
					ChiselMaterialManager.DefaultPhysicsMaterial
				)
			};

			brush = BrushMeshFactory.CreateBox(
				bounds.min,
				bounds.max,
				in surface
			);

			return brush;

		}
		return null; // Replace this
	}
}