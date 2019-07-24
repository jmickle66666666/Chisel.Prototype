using UnityEngine;
using UnityEditor;
using System;

namespace ChiselHandles {

    public enum ChiselHandleState { 
        Processing,
        Finished,
        Cancelled
    }
    
    public interface IChiselHandle
    {
        ChiselHandleState OnSceneGUI(SceneView sceneView);
    }

    public class Creators {

        public class DrawRectangle : IChiselHandle
        {
            Func<Vector3[], IChiselHandle> nextHandle;
            bool done = false;

            public DrawRectangle(Func<Vector3[], IChiselHandle> nextHandle) { 
                this.nextHandle = nextHandle;
            }

            public static IChiselHandle Create(Func<Vector3[], IChiselHandle> nextHandle)
            {
                return new DrawRectangle(nextHandle);
            }

            public ChiselHandleState OnSceneGUI(SceneView sceneView)
            {
                // Work some magic

                if (done) { 
                    return ChiselHandleState.Finished;
                }
                return ChiselHandleState.Processing;
            }
        }

        public class Extrude : IChiselHandle
        {
            Func<float, IChiselHandle> nextHandle;
            bool done = false;
            Vector3[] shape;

            public Extrude(Vector3[] shape, Func<float, IChiselHandle> nextHandle) { 
                this.shape = shape;
                this.nextHandle = nextHandle;
            }

            public static IChiselHandle Create(Vector3[] shape, Func<float, IChiselHandle> nextHandle)
            {
                return new Extrude(shape, nextHandle);
            }

            public ChiselHandleState OnSceneGUI(SceneView sceneView)
            {
                // Work some magic

                if (done) { 
                    return ChiselHandleState.Finished;
                }
                return ChiselHandleState.Processing;
            }
        }

    }

    namespace Modifiers {

        public class BoundsHandle : IChiselHandle
        {
            Func<Bounds, IChiselHandle> nextHandle;
            bool done = false;
            Bounds bounds;

            public BoundsHandle(Bounds bounds, Func<Bounds, IChiselHandle> nextHandle) { 
                this.bounds = bounds;
                this.nextHandle = nextHandle;
            }

            public static IChiselHandle Create(Bounds bounds, Func<Bounds, IChiselHandle> nextHandle)
            {
                return new BoundsHandle(bounds, nextHandle);
            }

            public ChiselHandleState OnSceneGUI(SceneView sceneView)
            {
                // Work some magic

                return ChiselHandleState.Processing;
            }
        }

    }


}
