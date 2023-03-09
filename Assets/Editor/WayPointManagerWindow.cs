using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WayPointManagerWindow : EditorWindow
{
    [MenuItem("Tools/WayPoint Editor")]
    public static void Open()
    {
        GetWindow<WayPointManagerWindow>();
    }

    public Transform waypointRoot;

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);

        EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));

        if (waypointRoot == null)
        {
            EditorGUILayout.HelpBox("root transform must be selected. Please assign a root transform", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            DrawButton();
            EditorGUILayout.EndVertical();
        }

        obj.ApplyModifiedProperties();
    }

    void DrawButton()
    {
        if (GUILayout.Button("Create Waypoint"))
        {
            CreateWaypoint();
        }

        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<WayPoint>())
        {
            if (GUILayout.Button("Add Branch Waypoint"))
            {
                CreateBranch();
            }
            if (GUILayout.Button("Create Waypoint Before"))
            {
                CreateWaypointBefore();
            }
            if (GUILayout.Button("Create Waypoint After"))
            {
                CreateWaypointAfter();
            }
            if (GUILayout.Button("Remove Waypoint"))
            {
                RemoveWaypoint();
            }
        }
    }

    void CreateWaypoint()
    {
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(WayPoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        WayPoint wayPoint = waypointObject.GetComponent<WayPoint>();
        if (waypointRoot.childCount > 1)
        {
            wayPoint.m_PreviousWayPoint = waypointRoot.GetChild(waypointRoot.childCount - 2).GetComponent<WayPoint>();
            wayPoint.m_PreviousWayPoint.m_NextWayPoint = wayPoint;

            wayPoint.transform.position = wayPoint.m_PreviousWayPoint.transform.position;
            wayPoint.transform.forward = wayPoint.m_PreviousWayPoint.transform.forward;
        }

        Selection.activeObject = wayPoint.gameObject;
    }

    void CreateWaypointBefore()
    {
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(WayPoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        WayPoint newWaypoint = waypointObject.GetComponent<WayPoint>();

        WayPoint selectedWaypoint = Selection.activeGameObject.GetComponent<WayPoint>();

        waypointObject.transform.position = selectedWaypoint.transform.position;
        waypointObject.transform.forward = selectedWaypoint.transform.forward;

        if (selectedWaypoint.m_PreviousWayPoint != null)
        {
            newWaypoint.m_PreviousWayPoint = selectedWaypoint.m_PreviousWayPoint;
            selectedWaypoint.m_PreviousWayPoint.m_NextWayPoint = newWaypoint;
        }

        newWaypoint.m_NextWayPoint = selectedWaypoint;

        selectedWaypoint.m_PreviousWayPoint = newWaypoint;

        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

        Selection.activeGameObject = newWaypoint.gameObject;
    }

    void CreateWaypointAfter()
    {
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(WayPoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        WayPoint newWaypoint = waypointObject.GetComponent<WayPoint>();

        WayPoint selectedWaypoint = Selection.activeGameObject.GetComponent<WayPoint>();

        waypointObject.transform.position = selectedWaypoint.transform.position;
        waypointObject.transform.forward = selectedWaypoint.transform.forward;

        newWaypoint.m_PreviousWayPoint = selectedWaypoint;

        if (selectedWaypoint.m_NextWayPoint != null)
        {
            selectedWaypoint.m_NextWayPoint.m_PreviousWayPoint = newWaypoint;
            newWaypoint.m_NextWayPoint = selectedWaypoint.m_NextWayPoint;
        }

        selectedWaypoint.m_NextWayPoint = newWaypoint;

        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

        Selection.activeGameObject = newWaypoint.gameObject;
    }

    void RemoveWaypoint()
    {
        WayPoint selectedWaypoint = Selection.activeGameObject.GetComponent<WayPoint>();

        if (selectedWaypoint.m_NextWayPoint != null)
        {
            selectedWaypoint.m_NextWayPoint.m_PreviousWayPoint = selectedWaypoint.m_PreviousWayPoint;
        }

        if (selectedWaypoint.m_PreviousWayPoint != null)
        {
            selectedWaypoint.m_PreviousWayPoint.m_NextWayPoint = selectedWaypoint.m_NextWayPoint;
            Selection.activeGameObject = selectedWaypoint.m_PreviousWayPoint.gameObject;
        }

        DestroyImmediate(selectedWaypoint.gameObject);
    }

    void CreateBranch()
    {
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(WayPoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        WayPoint waypoint = waypointObject.GetComponent<WayPoint>();

        WayPoint branchedFrom = Selection.activeGameObject.GetComponent<WayPoint>();
        branchedFrom.branches.Add(waypoint);

        waypoint.transform.position = branchedFrom.transform.position;
        waypoint.transform.forward = branchedFrom.transform.forward;

        Selection.activeGameObject = waypoint.gameObject;
    }
}
