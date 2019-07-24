using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using Chisel.Editors;
using Chisel.Components;

namespace ChiselHandles {

    public enum ChiselHandleState { 
        Processing,
        Finished,
        Cancelled
    }
    
    public interface IChiselHandle
    {
        ChiselHandleState OnSceneGUI(SceneView sceneView, Rect dragArea);
        IChiselHandle Next();
    }

    public class Creators {

        public class DrawRectangle : IChiselHandle
        {
            public Func<Vector3[], IChiselHandle> nextHandle;
            bool done = false;

            List<Vector3> points = new List<Vector3>();

            public DrawRectangle(Func<Vector3[], IChiselHandle> nextHandle) { 
                this.nextHandle = nextHandle;
            }

            public static IChiselHandle Create(Func<Vector3[], IChiselHandle> nextHandle)
            {
                return new DrawRectangle(nextHandle);
            }

            public IChiselHandle Next()
            {
                if (nextHandle == null) return null;

                Vector3[] rectangle = new Vector3[4]; // calculate output rectangle
                return nextHandle(rectangle);
            }

            public ChiselHandleState OnSceneGUI(SceneView sceneView, Rect dragArea)
            {
                // Work some magic

                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape) { 
                    return ChiselHandleState.Cancelled;
                }

                if (Event.current.type == EventType.MouseDown) { 
                    points.Add(Vector3.zero); // add current mouse point
                }

                if (points.Count == 2) { 
                    return ChiselHandleState.Finished;
                }
                return ChiselHandleState.Processing;
            }
        }

        public class Extrude : IChiselHandle
        {
            public Func<Vector3[], IChiselHandle> nextHandle;
            Vector3[] shape;
            List<Vector3> extrudePoints;

            public Extrude(Vector3[] shape, Func<Vector3[], IChiselHandle> nextHandle) { 
                this.shape = shape;
                this.nextHandle = nextHandle;

                extrudePoints = new List<Vector3>(2);
                
                extrudePoints[0] = shape[0];
                for (int i = 1; i < shape.Length; i++) {
                    extrudePoints[0] += shape[i];
                }
                extrudePoints[0] /= shape.Length;

                extrudePoints[1] = extrudePoints[0];
            }

            public static IChiselHandle Create(Vector3[] shape, Func<Vector3[], IChiselHandle> nextHandle)
            {
                return new Extrude(shape, nextHandle);
            }

            public ChiselHandleState OnSceneGUI(SceneView sceneView, Rect dragArea)
            {
                // Work some magic
                PointDrawing.PointDrawHandle(dragArea, ref extrudePoints, out Matrix4x4 transformation, out ChiselModel model, UnitySceneExtensions.SceneHandles.OutlinedDotHandleCap);

                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape) { 
                    return ChiselHandleState.Cancelled;
                }

                if (Event.current.type == EventType.MouseUp) { 
                    return ChiselHandleState.Finished;
                }
                return ChiselHandleState.Processing;
            }

            public IChiselHandle Next()
            {
                if (nextHandle == null) {
                    return null;
                }

                Vector3[] outPoints = new Vector3[shape.Length * 2];
                for (int i = 0; i < shape.Length; i++) {
                    outPoints[i] = shape[i];
                    outPoints[i+shape.Length] = shape[i] + extrudePoints[1] - extrudePoints[0];
                }

                return nextHandle(outPoints);
            }
        }

    }

    namespace Modifiers {

        public class BoundsHandle : IChiselHandle
        {
            public Func<Bounds, IChiselHandle> nextHandle;
            Bounds bounds;
            UnityEngine.Object target;

            public BoundsHandle(Bounds bounds, Func<Bounds, IChiselHandle> nextHandle) { 
                this.bounds = bounds;
                this.nextHandle = nextHandle;
            }

            public static IChiselHandle Create(Bounds bounds, Func<Bounds, IChiselHandle> nextHandle)
            {
                return new BoundsHandle(bounds, nextHandle);
            }

            public ChiselHandleState OnSceneGUI(SceneView sceneView, Rect dragArea)
            {
                // Work some magic
                EditorGUI.BeginChangeCheck();

                var newBounds = bounds;
                newBounds = UnitySceneExtensions.SceneHandles.BoundsHandle(newBounds, Quaternion.identity);
                
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, $"Modified {target.name}");
                    bounds = newBounds;
                }

                return ChiselHandleState.Processing;
            }

            public IChiselHandle Next()
            {
                return null;
            }
        }

    }


}
