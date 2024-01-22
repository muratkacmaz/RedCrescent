using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(LineRenderer))]
public class Bezier : MonoBehaviour
{
    public bool endPointDetected;
    public Material linemat,curveMat;

    public Vector3 EndPoint
    {
        get { return endpoint; }
    }

    public float ExtensionFactor
    {
        set { extensionFactor = value; }
    }

    public bool linearLine;

    private Vector3 endpoint;
    public float extensionFactor;
    private Vector3[] controlPoints;
    public LineRenderer lineRenderer;
    private float extendStep;
    private int SEGMENT_COUNT = 50;
    public GameObject seenObj;

    void Start()
    {
        controlPoints = new Vector3[3];
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        extendStep = 5f;
        extensionFactor = 0f;
        linearLine = false;
    }

    void Update()
    {
        UpdateControlPoints();
        HandleExtension();
        DrawCurve();
    }

    public void ToggleDraw(bool draw)
    {
        lineRenderer.enabled = draw;
    }

    void HandleExtension()
    {
        if (extensionFactor == 0f)
            return;

        float finalExtension = extendStep + Time.deltaTime * extensionFactor * 2f;
        extendStep = Mathf.Clamp(finalExtension, 2.5f, 7.5f);
    }

    public void bezierIsLinear()
    {
        linearLine = true;
        lineRenderer.material = linemat;
        lineRenderer.startWidth = 0.001f;
        lineRenderer.endWidth = 0.001f;
    }
    public void bezierIsCurve()
    {
        linearLine = false;
        lineRenderer.material = curveMat;
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
    }


    // The first control is the remote. The second is a forward projection. The third is a forward and downward projection.
    void UpdateControlPoints()
    {
        controlPoints[0] = gameObject.transform.position; // Get Controller Position
        controlPoints[1] = controlPoints[0] + (gameObject.transform.forward * extendStep * 2f / 5f);
        if (linearLine == false)
        {
            controlPoints[2] = controlPoints[1] + (gameObject.transform.forward * extendStep * 3f / 5f) + Vector3.up * -1f;
        }
    }


    // Draw the bezier curve.
    void DrawCurve()
    {
        if (!lineRenderer.enabled)
            return;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, controlPoints[0]);

        Vector3 prevPosition = controlPoints[0];
        Vector3 nextPosition = prevPosition;
        for (int i = 1; i <= SEGMENT_COUNT; i++)
        {
            float t = i / (float)SEGMENT_COUNT;
            lineRenderer.positionCount = i + 1;

            if (i == SEGMENT_COUNT)
            { // For the last point, project out the curve two more meters.
                Vector3 endDirection = Vector3.Normalize(prevPosition - lineRenderer.GetPosition(i - 2));
                nextPosition = prevPosition + endDirection * 2f;
            }
            else
            {
                if (linearLine == false)
                    nextPosition = CalculateBezierPoint(t, controlPoints[0], controlPoints[1], controlPoints[2]);
                else if (linearLine == true)
                    nextPosition = CalculateLinearBezierPoint(t, controlPoints[0], controlPoints[1]);
            }

            if (CheckColliderIntersection(prevPosition, nextPosition))
            { // If the segment intersects a surface, draw the point and return.
                lineRenderer.SetPosition(i, endpoint);
                endPointDetected = true;
                return;
            }
            else
            { // If the point does not intersect, continue to draw the curve.
                lineRenderer.SetPosition(i, nextPosition);
                endPointDetected = false;
                prevPosition = nextPosition;
            }
        }
    }

    // Check if the line between start and end intersect a collider.
    bool CheckColliderIntersection(Vector3 start, Vector3 end)
    {
        
        Ray r = new Ray(start, end - start);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, Vector3.Distance(start, end)))
        {
            endpoint = hit.point;
             PlayerPrefs.SetString("bakilanObj", hit.transform.name) ;
            seenObj = hit.transform.gameObject;

           // Debug.Log("objSetAS " + hit.transform.name);
            return true;
        }

        return false;
    }

    public GameObject getHitObject()
    {
        return seenObj;

    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return
            Mathf.Pow((1f - t), 2) * p0 + 2f * (1f - t) * t * p1 + Mathf.Pow(t, 2) * p2;
    }
    Vector3 CalculateLinearBezierPoint(float t, Vector3 p0, Vector3 p1)
    {
        return
            (1f - t) * p0 + p1 * t;

    }
}
