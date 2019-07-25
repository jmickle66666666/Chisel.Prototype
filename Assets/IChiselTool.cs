using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChiselHandles;
using Chisel.Editors;

public interface IChiselTool
{
    string name { get; }
    IChiselHandle OnCreate();
    IChiselHandle OnModify();
    void OnBuild();
}
