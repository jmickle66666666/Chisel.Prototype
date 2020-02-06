using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Extract/Texture")]
public class TextureDataNode : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	[Input] public Texture2D texture;
	[Output] public Vector2 sizeFloat;
	[Output] public Vector2Int sizeInt;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {

		texture = GetInputValue<Texture2D>("texture", texture);

		if (port.fieldName == "sizeFloat") {
			return new Vector2(
				texture.width,
				texture.height
			);
		}

		if (port.fieldName == "sizeInt") {
			return new Vector2Int(
				texture.width,
				texture.height
			);
		}

		return null; // Replace this
	}
}