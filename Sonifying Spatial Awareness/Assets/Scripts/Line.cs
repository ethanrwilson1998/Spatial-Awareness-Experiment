using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line
{
    private Vector3 start;
    private Vector3 end;

    public Line(Vector3 s, Vector3 e)
    {
        start = s;
        end = e;
    }

    public float Distance(Vector3 point)
    {
        Vector3 direction = (end - start).normalized;


        return Vector3.Cross(direction, point - start).magnitude;

    }
}
