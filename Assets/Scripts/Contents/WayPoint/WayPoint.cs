using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public WayPoint m_PreviousWayPoint;
    public WayPoint m_NextWayPoint;

    [Range(0f, 5f)]
    public float m_Width = 1f;

    public List<WayPoint> branches;

    [Range(0f, 1f)]
    public float branchRatio = 0.5f;

    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * m_Width / 2f;
        Vector3 maxBound = transform.position + transform.right * m_Width / 2f;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }
}
