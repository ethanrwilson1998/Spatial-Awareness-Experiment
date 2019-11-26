using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private bool interpolate;
    [SerializeField] private float interpCutoff;
    private Line line;
    private int idx;

    // just for debug visuals
    private LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();

        if (interpolate)
        {
            InterpolatePoints();
        }

        NextLine();

    }

    private void InterpolatePoints()
    {
        bool didStuff = true;
        while (didStuff)
        {
            didStuff = false;

            List<Transform> pts = new List<Transform>();
            for (int i = 0; i < points.Length - 1; i++)
            {
                pts.Add(points[i]);
                if ((points[i].position - points[i + 1].position).magnitude > interpCutoff)
                {
                    Transform newPt = Instantiate(points[i].gameObject).transform;
                    newPt.position = (points[i].position + points[i + 1].position) / 2f;
                    pts.Add(newPt);
                    didStuff = true;
                }
            }
            pts.Add(points[points.Length - 1]);

            points = pts.ToArray();
        }
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

    public float TotalDistance()
    {
        float d = 0;
        for (int i = 0; i < points.Length - 1; i++)
        {
            d += (points[i].position - points[i + 1].position).magnitude;
        }
        return d;
    }

    public int GetNumPoints()
    {
        return points.Length;
    }

    public int GetIndex()
    {
        return idx;
    }
}
