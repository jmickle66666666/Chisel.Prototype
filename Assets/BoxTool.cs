using ChiselHandles;
using UnityEngine;

class BoxTool : IChiselTool
{

    public string name {get { return "BoxTool"; } }

    #region Creator

    Bounds boxBounds = new Bounds();
    
    public IChiselHandle OnCreate()
    {
        return ChiselHandles.Creators.DrawRectangle.Create(
            (rect) => { return ChiselHandles.Creators.Extrude.Create(rect,
                (boxPoints) => {
                    // this.boxBounds = GetBoundsFromExtrusion(rect, length);

                    Matrix4x4 transformation = UnitySceneExtensions.Grid.HoverGrid.GridToWorldSpace;

                    Vector3 center = boxPoints[0];
                    for (int i = 1; i < boxPoints.Length; i++) {
                        center += boxPoints[i];
                    }
                    center /= boxPoints.Length;

                    center = transformation.MultiplyPoint(center);


                    Vector3 size = new Vector3(
                        Mathf.Abs(boxPoints[0].x - boxPoints[2].x),
                        Mathf.Abs(boxPoints[0].z - boxPoints[2].z),
                        Mathf.Abs(boxPoints[0].y - boxPoints[4].y)
                    );

                    size = transformation.MultiplyPoint(size);


                    boxBounds = new Bounds(center, size);

                    return null;
                } );
            }
        );
    }

    #endregion

    #region Modifier

    public IChiselHandle OnModify()
    {
        return ChiselHandles.Modifiers.BoundsHandle.Create (boxBounds, (newBounds) => { this.boxBounds = newBounds; return null; });
    }

    #endregion

    #region Procedure

    public void OnBuild()
    {
        // Chisel.Core.BrushMeshFactory.GenerateBox(boxBounds);
    }

    #endregion

}