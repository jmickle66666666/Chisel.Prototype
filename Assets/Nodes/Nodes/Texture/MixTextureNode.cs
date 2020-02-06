using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public enum TextureMixMethod {
	Mix,
	Add,
	Subtract
}

[CreateNodeMenu("Texture/Mix")]
public class MixTextureNode : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	[Input] public Texture2D a;
	[Input] public Texture2D b;
	[Input] public TextureMixMethod mixMethod;
	[Input] public float amount = 0.5f;
	[Output] public Texture2D output;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {

		if (port.fieldName == "output") {

			a = GetInputValue<Texture2D>("a", a);
			b = GetInputValue<Texture2D>("b", b);
			mixMethod = GetInputValue<TextureMixMethod>("b", mixMethod);
			amount = GetInputValue<float>("amount", amount);

			return Mix(a, b, amount);

		}

		return null; // Replace this
	}

	Texture2D Mix(Texture2D a, Texture2D b, float amount) {

		float maxWidth = Mathf.Max(a.width, b.width);
		float maxHeight = Mathf.Max(a.height, b.height);

		Texture2D output = new Texture2D(
			Mathf.FloorToInt(maxWidth),
			Mathf.FloorToInt(maxHeight)
		);

		// Vector2 uvA = new Vector2();
		// Vector2 uvB = new Vector2();
		for (float i = 0; i < 1f; i += 1f/maxWidth) {
			for (float j = 0; j < 1f; j+= 1f/maxHeight) {
				Color A = a.GetPixelBilinear(i, j);
				Color B = b.GetPixelBilinear(i, j);

				output.SetPixel(
					Mathf.FloorToInt(i * maxWidth), 
					Mathf.FloorToInt(j * maxHeight), 
					Color.Lerp(A, B, amount)
				);
			}
		}

		output.Apply();
		return output;
	}
}