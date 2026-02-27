using System;
using UnityEngine;

[Serializable]
public struct Symbol
{
    public char C;
    public float P0;
    public bool HasP0;

    public Symbol(char c)
    {
        C = c;
        P0 = 0f;
        HasP0 = false;
    }

    public Symbol(char c, float p0)
    {
        C = c;
        P0 = p0;
        HasP0 = true;
    }

    public override string ToString()
    {
        return HasP0 ? $"{C}({P0:0.###})" : C.ToString();
    }
}