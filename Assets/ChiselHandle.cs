using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using Chisel.Editors;
using Chisel.Components;
using UnitySceneExtensions;

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
        int controlHash {get;}
    }

    public class Creators {

        public class DrawRectangle : IChiselHandle
        {
            public Func<Vector3[], IChiselHandle> nextHandle;
            bool done = false;
            public int controlHash { get { return "DrawRectangle".GetHashCode(); }}

            List<Vector3> points = new List<Vector3>();
            Matrix4x4 transformation;

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

                return nextHandle(GetRectangle());
            }

            Vector3[] GetRectangle() {
                if (points.Count < 2) return null;

                return new Vector3[4] {
                    points[0], 
                    new Vector3(points[0].x, 0f, points[1].z),
                    points[1],
                    new Vector3(points[1].x, 0f, points[0].z),
                };
            }

            public ChiselHandleState OnSceneGUI(SceneView sceneView, Rect dragArea)
            {
                // Work some magic


                var evt = Event.current;

                if (evt.type == EventType.Layout) { 
                    // UnityEditor.HandleUtility.AddControl(GUIUtility.GetControlID(controlHash, FocusType.Keyboard), 100f);
                }

                if (evt.type == EventType.MouseUp && GUIUtility.hotControl != 0) { 
                    if (points.Count >= 2) { 
                        return ChiselHandleState.Finished;
                    }
                    evt.Use();
                }

                PointDrawing.PointDrawHandle(dragArea, ref points, out Matrix4x4 transformation, out ChiselModel model, UnitySceneExtensions.SceneHandles.OutlinedDotHandleCap);
                this.transformation = transformation;

                if (evt.type == EventType.Repaint) { 
                    
                    if (points.Count == 2) {
                        Vector3[] rectangle = GetRectangle();
                        SceneHandles.DrawLine(transformation * rectangle[0], transformation * rectangle[1]);
                        SceneHandles.DrawLine(transformation * rectangle[1], transformation * rectangle[2]);
                        SceneHandles.DrawLine(transformation * rectangle[2], transformation * rectangle[3]);
                        SceneHandles.DrawLine(transformation * rectangle[3], transformation * rectangle[0]);
                    }
                
                }


                if (evt.type == EventType.KeyDown && evt.keyCode == KeyCode.Escape) { 
                    evt.Use();
                    return ChiselHandleState.Cancelled;
                }


                return ChiselHandleState.Processing;
            }
        }

        public class Extrude : IChiselHandle
        {
            public int controlHash { get { return "Extrude".GetHashCode(); }}
            public Func<Vector3[], IChiselHandle> nextHandle;
            Vector3[] shape;
            // List<Vector3> extrudePoints;
            Vector3 center;
            Vector3 extrude;
            Vector2 mousePosition;

            Snapping1D snapping = new Snapping1D();

            public Extrude(Vector3[] shape, Func<Vector3[], IChiselHandle> nextHandle) { 
                this.shape = shape;
                this.nextHandle = nextHandle;
                
                center = shape[0];
                for (int i = 1; i < shape.Length; i++) {
                    center += shape[i];
                }
                center /= shape.Length;

                extrude = Vector3.zero;
            }

            public static IChiselHandle Create(Vector3[] shape, Func<Vector3[], IChiselHandle> nextHandle)
            {
                return new Extrude(shape, nextHandle);
            }

            public ChiselHandleState OnSceneGUI(SceneView sceneView, Rect dragArea)
            {
                // Work some magic
                // PointDrawing.PointDrawHandle(dragArea, ref extrudePoints, out Matrix4x4 transformation, out ChiselModel model, UnitySceneExtensions.SceneHandles.OutlinedDotHandleCap);

                var evt = Event.current;
                int id = GUIUtility.GetControlID(controlHash, FocusType.Keyboard);

                if (evt.type == EventType.Layout) { 
                    UnityEditor.HandleUtility.AddControl(id, 0.0f);
                }
                Matrix4x4 transformation = UnitySceneExtensions.Grid.HoverGrid.GridToWorldSpace;
                if (evt.type == EventType.Repaint) { 
                    for (int i = 0; i < shape.Length; i++) {
                        SceneHandles.DrawLine(transformation.MultiplyPoint(shape[i]), transformation.MultiplyPoint(shape[(i+1)%shape.Length]));
                        SceneHandles.DrawLine(transformation.MultiplyPoint(shape[i]), transformation.MultiplyPoint(shape[i] + extrude));
                        SceneHandles.DrawLine(transformation.MultiplyPoint(shape[i] + extrude), transformation.MultiplyPoint(shape[(i+1)%shape.Length] + extrude));
                    }
                }

                if (evt.type == EventType.MouseDown && GUIUtility.hotControl == id) { 
                    evt.Use();
                    return ChiselHandleState.Finished;
                }

                if (evt.type == EventType.MouseMove || evt.type == EventType.MouseMove) {
                    if (GUIUtility.hotControl == 0) { 
                        GUIUtility.hotControl = id;
                        GUIUtility.keyboardControl = id;
                        EditorGUIUtility.SetWantsMouseJumping(1);
                        mousePosition = evt.mousePosition - evt.delta;
                        snapping.Initialize(evt.mousePosition, 
                                                            transformation * shape[0], 
                                                            ((Vector3) transformation.GetColumn(1)).normalized,
                                                            Snapping.MoveSnappingSteps.y, Axis.Y);
                    }

                    if (GUIUtility.hotControl != id) {
                        return ChiselHandleState.Processing;
                    }

                    mousePosition += evt.delta;
                    snapping.Move(mousePosition);

                    extrude = transformation.inverse.MultiplyPoint(snapping.WorldSnappedPosition) - shape[0];
                }

                if (evt.type == EventType.KeyDown && evt.keyCode == KeyCode.Escape) { 
                    evt.Use();
                    return ChiselHandleState.Cancelled;
                }

                // var extrusionState = ExtrusionHandle.DoHandle(dragArea, ref extrude, Axis.Y);
                // if (extrusionState == ExtrusionState.Commit) {
                //     return ChiselHandleState.Finished;
                // }


                return ChiselHandleState.Processing;
            }

            public IChiselHandle Next()
            {
                if (nextHandle == null) {
                    return null;
                }

                Matrix4x4 transformation = UnitySceneExtensions.Grid.HoverGrid.GridToWorldSpace;
                Vector3[] outPoints = new Vector3[shape.Length * 2];
                for (int i = 0; i < shape.Length; i++) {
                    outPoints[i] = shape[i];
                    outPoints[i+shape.Length] = shape[i] + extrude;
                }

                return nextHandle(outPoints);
            }
        }

    }

    namespace Modifiers {

        public class BoundsHandle : IChiselHandle
        {
            public int controlHash { get { return "BoundsHandle".GetHashCode(); }}
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
