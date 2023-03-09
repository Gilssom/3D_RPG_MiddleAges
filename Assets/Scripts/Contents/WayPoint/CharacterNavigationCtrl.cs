using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNavigationCtrl : MonoBehaviour
{
    public float MovementSpeed;
    public float RotationSpeed;
    public float StopDistance;
    public Vector3 Destination;
    public bool ReachedDestination;

    private void Update()
    {
        if (transform.position != Destination)
        {
            Vector3 destinationDierection = Destination - transform.position;
            destinationDierection.y = 0;

            float destinationDistance = destinationDierection.magnitude;

            if (destinationDistance >= StopDistance)
            {
                ReachedDestination = false;
                Quaternion targetRotation = Quaternion.LookRotation(destinationDierection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * MovementSpeed * Time.deltaTime);
            }
            else
            {
                ReachedDestination = true;
            }
        }
    }

    public void SetDestination(Vector3 destination)
    {
        this.Destination = destination;
        ReachedDestination = false;
    }
}
