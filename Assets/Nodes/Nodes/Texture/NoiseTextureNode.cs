using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Texture/Noise")]
public class NoiseTextureNode : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	[Input] public Vector2Int size = new Vector2Int(64, 64);
	[Input] public int seed;
	[Output] public Texture2D texture;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		seed = GetInputValue<int>("seed", Random.Range(0,32767));
		if (port.fieldName == "texture") {

			Random.InitState(seed);

			size = GetInputValue<Vector2Int>("size", size);
			Texture2D outputTexture = new Texture2D(
				size.x,
				size.y
			);

			Color32[] colors = outputTexture.GetPixels32();
			for (int i = 0; i < colors.Length; i++) {
				byte rand = (byte)Random.Range(0, 256);
				colors[i] = new Color32(rand, rand, rand, 255);
			}
			outputTexture.SetPixels32(colors);
			outputTexture.Apply();

			return outputTexture;

		}
		return null; // Replace this
	}
}