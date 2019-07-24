using ChiselHandles;
using UnityEngine;

class BoxTool : IChiselTool
{

    #region Creator

    Bounds boxBounds = new Bounds();
    
    public IChiselHandle OnCreate()
    {
        return ChiselHandles.Creators.DrawRectangle.Create(
            (rect) => { return ChiselHandles.Creators.Extrude.Create(rect,
                (length) => {
                    // this.boxBounds = GetBoundsFromExtrusion(rect, length);
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
        //BrushMeshFactory.GenerateBox(boxBounds);
    }

    #endregion

}