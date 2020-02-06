using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Texture/Layered Noise")]
public class LayeredNoise : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	[Input] public Vector2Int size = new Vector2Int(64, 64);
	[Input] public int seed;
	[Input] public int octaves = 2;
	[Input] public float falloff = 0.5f;
	[Input] public bool normalize = true;
	[Output] public Texture2D texture;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {

		seed = GetInputValue<int>("seed", Random.Range(0,32767));
		falloff = GetInputValue<float>("falloff", falloff);
		octaves = GetInputValue<int>("octaves", octaves);
		normalize = GetInputValue<bool>("normalize", normalize);
		if (port.fieldName == "texture") {

			Random.InitState(seed);

			var tsize = GetInputValue<Vector2Int>("size", size);

			Texture2D[] textures = new Texture2D[octaves];
			
			for (int i = 0; i < octaves; i++) {
				textures[i] = GenerateNoiseTexture(tsize);
				tsize.x /= 2;
				tsize.y /= 2;
			}


			for (int x = 0; x < textures[0].width; x++) {
				for (int y = 0; y < textures[0].height; y++) {
					for (int i = 1; i < octaves; i++) {
						float u = (float) x / (float) textures[0].width;
						float v = (float) y / (float) textures[0].height;

						var iPix = textures[i].GetPixelBilinear(u, v);
						var uPix = textures[0].GetPixelBilinear(u, v);
						var lPix = Color.Lerp(
							iPix,
							uPix,
							falloff
						);

						textures[0].SetPixel(
							x, y,
							lPix
						);
					}
					textures[0].Apply();
				}
			}

			if (normalize) {

				float min = 1f;
				float max = 0f;
				for (int x = 0; x < textures[0].width; x++) {
					for (int y = 0; y < textures[0].height; y++) {
						float val = textures[0].GetPixel(x, y).r;
						if (val < min) min = val;
						if (val > max) max = val;
					}
				}

				float range = max - min;
				for (int x = 0; x < textures[0].width; x++) {
					for (int y = 0; y < textures[0].height; y++) {

						float f = (textures[0].GetPixel(x, y).r - min) * (1f/range);
						textures[0].SetPixel(x, y, 
							new Color(f,f,f,1.0f)
						);
					}
				}
				textures[0].Apply();

			}

			

			return textures[0];

		}

		return null; // Replace this
	}

	Texture2D GenerateNoiseTexture(Vector2Int dimensions)
	{
		Texture2D outputTexture = new Texture2D(
			Mathf.Max(dimensions.x, 1),
			Mathf.Max(dimensions.y, 1)
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
}