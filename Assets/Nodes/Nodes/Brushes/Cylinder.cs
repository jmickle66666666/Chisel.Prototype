using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Chisel.Core;

[CreateNodeMenu("Brush/Cylinder")]
public class Cylinder : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	[Input] public Bounds bounds;
	public CylinderShapeType shapeType;
	[Input] public int sides;
	[Input] public float angle;
	[Output] public BrushMesh brush;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {

		if (port.fieldName == "brush") {
			bounds = GetInputValue<Bounds>("bounds", bounds);
			sides = GetInputValue<int>("sides", sides);
			angle = GetInputValue<float>("angle", angle);

			brush = new BrushMesh();

			var circleBase = new ChiselCircleDefinition(
				bounds.size.x, bounds.size.z, -bounds.extents.y
			);

			var circleTop = new ChiselCircleDefinition(
				bounds.size.x, bounds.size.z, bounds.extents.y
			);

			var definition = new ChiselCylinderDefinition() {
				top = circleTop,
				bottom = circleBase,
				isEllipsoid = true,
				type = shapeType,
				sides = sides,
				rotation = angle
			};

			BrushMeshFactory.GenerateCylinder(ref brush, ref definition);

			var vertices = brush.vertices;
			for (int i = 0; i < vertices.Length; i++) {
				vertices[i] += bounds.center;
			}
			brush.vertices = vertices;

			return brush;
		}

		return null; // Replace this
	}
}