using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChiselHandles;

public interface IChiselTool
{
    IChiselHandle OnCreate();
    IChiselHandle OnModify();
    void OnBuild();
}
