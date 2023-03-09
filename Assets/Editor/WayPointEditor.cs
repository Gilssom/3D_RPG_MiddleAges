using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad()]
public class WayPointEditor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmo(WayPoint waypoint, GizmoType gizmoType)
    {
        if ((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.yellow * 0.5f;
        }

        Gizmos.DrawSphere(waypoint.transform.position, 1f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(waypoint.transform.position + (waypoint.transform.right * waypoint.m_Width / 2f),
            waypoint.transform.position - (waypoint.transform.right * waypoint.m_Width / 2f));

        if (waypoint.m_PreviousWayPoint != null)
        {
            Gizmos.color = Color.red;
            Vector3 offset = waypoint.transform.right * waypoint.m_Width / 2f;
            Vector3 offsetTo = waypoint.m_PreviousWayPoint.transform.right * waypoint.m_PreviousWayPoint.m_Width / 2f;

            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.m_PreviousWayPoint.transform.position + offsetTo);
        }

        if (waypoint.m_NextWayPoint != null)
        {
            Gizmos.color = Color.green;
            Vector3 offset = waypoint.transform.right * -waypoint.m_Width / 2f;
            Vector3 offsetTo = waypoint.m_NextWayPoint.transform.right * -waypoint.m_NextWayPoint.m_Width / 2f;

            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.m_NextWayPoint.transform.position + offsetTo);
        }

        if (waypoint.branches != null)
        {
            foreach (WayPoint branch in waypoint.branches)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(waypoint.transform.position, branch.transform.position);
            }
        }
    }
}
