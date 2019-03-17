using Chisel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Chisel.Editors
{

	// TODO: add ability to store position (per sceneview?)
	// TODO: add ability to become dockable window?
	// TODO: add scrollbar support
	// TODO: use icons, make this look better
	public static class CSGSurfaceEditModeGUI
	{
		const float kSingleLineHeight = 20f;
		const float kSingleSpacing = 0.0f;

		static GUIResizableWindow editModeWindow;

		static string[] U = {"0","0","0","0"};
		static string[] V = {"0","0","0","0"};
		static void OnWindowGUI(Rect position)
		{
			position.height = kSingleLineHeight;
			position.width = position.width * 0.25f;

			U[0] = GUI.TextField(position, U[0]); position.x += position.width;
			U[1] = GUI.TextField(position, U[1]); position.x += position.width;
			U[2] = GUI.TextField(position, U[2]); position.x += position.width;
			U[3] = GUI.TextField(position, U[3]); 
			
			position.x -= position.width * 3f;
			position.y += kSingleLineHeight;

			V[0] = GUI.TextField(position, V[0]); position.x += position.width;
			V[1] = GUI.TextField(position, V[1]); position.x += position.width;
			V[2] = GUI.TextField(position, V[2]); position.x += position.width;
			V[3] = GUI.TextField(position, V[3]); 

			position.x -= position.width * 3f;
			position.y += kSingleLineHeight;

			position.width = position.width * 4f;
			if (GUI.Button(position, "apply")) { 
				Vector4 u = new Vector4(
					float.Parse(U[0]), 
					float.Parse(U[1]), 
					float.Parse(U[2]), 
					float.Parse(U[3]) 
				);
				Vector4 v = new Vector4(
					float.Parse(V[0]), 
					float.Parse(V[1]), 
					float.Parse(V[2]), 
					float.Parse(V[3]) 
				);
				CSGSurfaceEditMode.startSurfaceIntersection.surface.Polygon.description.UV0 = new UVMatrix(u, v);
				CSGSurfaceEditMode.startSurfaceIntersection.surface.brushMeshAsset.SetDirty();
			}
		}

		public static void OnSceneGUI(SceneView sceneView, Rect dragArea)
		{
			if (editModeWindow == null)
			{
				var minWidth	= 80;
				var minHeight	= 40;
				var rect		= new Rect(100, 100, 240, 120);
				editModeWindow = new GUIResizableWindow("Surface", rect, minWidth, minHeight, OnWindowGUI);
			}

			editModeWindow.Show(dragArea);
			
		}
	}
}
