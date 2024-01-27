using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SootSpawner : MonoBehaviour
{
    public SootSprite sootSpritePrefab;
    public float minSpawnInterval, maxSpawnInterval;

    private float nextSpawnTime = 0f;
    void Update()
    {
        if (JoyMeter.Instance.currentJoy < .8f && Time.time > nextSpawnTime)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        var spawnLocations = GameObject.FindGameObjectsWithTag("Soot Spawn Location");

        var desiredLocation = spawnLocations
                                .OrderBy(t => (Character.Instance.center - t.transform.position).magnitude)
                                .Skip(Random.Range(0,5))
                                .FirstOrDefault(t => !IsVisibleToCamera(t.transform));

        if (desiredLocation == null)
            desiredLocation = spawnLocations.OrderBy(t => Random.value).First();

        Instantiate(sootSpritePrefab, desiredLocation.transform.position, Quaternion.identity, this.transform);
        
        nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    bool IsVisibleToCamera(Transform target)
    {
        Vector3 direction = target.position - Camera.main.transform.position;
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, direction, out hit, direction.magnitude))
        {
            // Check if the hit object is not the target and is between the camera and the target
            if (hit.transform != target)
            {
                return true; // There is an obstruction
            }
        }
        return false; // No obstruction
    }
}
