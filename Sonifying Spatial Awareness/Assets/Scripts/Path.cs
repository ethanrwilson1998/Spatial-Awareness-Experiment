using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    private Line line;
    private int idx;

    // just for debug visuals
    private LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();

        NextLine();
    }

    public bool NextLine()
    {
        if (idx >= points.Length - 1)
        {
            return false;
        }
        lr.SetPosition(0, points[idx].position);
        lr.SetPosition(1, points[idx + 1].position);
        line = new Line(points[idx].position, points[++idx].position);
        return true;
    }

    public Vector3 Next()
    {
        if (idx < points.Length)
        {
            return points[idx].position;
        }
        return Vector3.zero;
    }

    public float Distance(Vector3 point)
    {
        return line.Distance(point);
    }
}
