using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class PreviewNode : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	[Input] public Object previewObject;

	
}