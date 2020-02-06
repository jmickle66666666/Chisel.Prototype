using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chisel.Components;
using Chisel.Core;

[ExecuteInEditMode]
public class BrushGenerator : MonoBehaviour
{
    public GeneratorGraph generator;
    public BrushMesh output;
    public ChiselBrush brush;

    public bool work = false;

    void Update()
    {
        if (work) {
            work = false;
            Work();
        }
    }

    void Work()
    {
        if (generator == null) {
            return;
        }

        output = generator.GetOutput();
        if (brush != null) {
            brush.BrushMesh = output;
        }
    }
}
