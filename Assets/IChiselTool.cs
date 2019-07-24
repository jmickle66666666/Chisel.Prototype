using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChiselHandles;
using Chisel.Editors;

public interface IChiselTool
{
    IChiselHandle OnCreate();
    IChiselHandle OnModify();
    void OnBuild();
}
