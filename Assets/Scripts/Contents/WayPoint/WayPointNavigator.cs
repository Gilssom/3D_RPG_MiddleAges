using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointNavigator : MonoBehaviour
{
    CharacterNavigationCtrl controller;
    public WayPoint curWayPoint;

    int direction;

    private void Awake()
    {
        controller = GetComponent<CharacterNavigationCtrl>();
    }

    private void Start()
    {
        direction = Mathf.RoundToInt(Random.Range(0f, 1f));
        controller.SetDestination(curWayPoint.GetPosition());
    }

    private void Update()
    {
        if (controller.ReachedDestination)
        {
            bool shouldBranch = false;

            if (curWayPoint.branches != null && curWayPoint.branches.Count > 0)
            {
                shouldBranch = Random.Range(0f, 1f) <= curWayPoint.branchRatio ? true : false;
            }
            if (shouldBranch)
            {
                curWayPoint = curWayPoint.branches[Random.Range(0, curWayPoint.branches.Count - 1)];
            }
            else
            {
                if (direction == 0)
                {
                    if (curWayPoint.m_NextWayPoint != null)
                    {
                        curWayPoint = curWayPoint.m_NextWayPoint;
                    }
                    else
                    {
                        curWayPoint = curWayPoint.m_PreviousWayPoint;
                        direction = 1;
                    }
                }
                else if (direction == 1)
                {
                    if (curWayPoint.m_PreviousWayPoint != null)
                    {
                        curWayPoint = curWayPoint.m_PreviousWayPoint;
                    }
                    else
                    {
                        curWayPoint = curWayPoint.m_NextWayPoint;
                        direction = 0;
                    }
                }
            }
            controller.SetDestination(curWayPoint.GetPosition());
        }
    }
}
