using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    public GameObject[] m_PeoplePrefab;
    public int m_PeopleToSpawn;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        int count = 0;
        while (count < m_PeopleToSpawn)
        {
            int RandomPeople = Random.Range(0, 5);
            GameObject obj = Instantiate(m_PeoplePrefab[RandomPeople]);
            Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
            obj.GetComponent<WayPointNavigator>().curWayPoint = child.GetComponent<WayPoint>();
            obj.transform.position = child.position;

            yield return new WaitForEndOfFrame();

            count++;
        }
    }
}
