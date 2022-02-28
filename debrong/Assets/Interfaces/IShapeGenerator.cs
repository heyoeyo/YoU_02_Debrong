using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShapeGenerator {
    public void GenerateShape(MinMaxFloat scale_range, MinMaxFloat jaggedness_range);
}