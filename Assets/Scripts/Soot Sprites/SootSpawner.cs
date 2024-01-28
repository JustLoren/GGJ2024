using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
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

    private List<GameObject> spawnLocations = new();
    private void OnEnable()
    {
        spawnLocations = GameObject.FindGameObjectsWithTag("Soot Spawn Location").ToList();
    }

    private void Spawn()
    {
        var orderedLocations = spawnLocations.OrderBy(t => (Character.Instance.center - t.transform.position).magnitude).ToList();

        var desiredLocation = orderedLocations
                                .Skip(Random.Range(0,5))
                                .FirstOrDefault(t => IsObstructedFromCamera(t.transform));

        if (desiredLocation == null)
        {
            desiredLocation = spawnLocations.OrderBy(t => Random.value).First();
            Debug.Log("Couldn't find a suitable spawn location. Choosing randomly.");
        } else
        {
            Debug.DrawLine(Camera.main.transform.position, desiredLocation.transform.position, Color.green, 1f);
        }

        Instantiate(sootSpritePrefab, desiredLocation.transform.position, Quaternion.identity, this.transform);
        
        nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    public LayerMask obstructionLayers;
    bool IsObstructedFromCamera(Transform target)
    {
        var pt = Camera.main.WorldToViewportPoint(target.position);
        if (pt.x < 0 || pt.x > 1 || pt.y < 0 || pt.y > 1 || pt.z < 0)
        {
            return true;
        }

        Vector3 direction = target.position - Camera.main.transform.position;
        RaycastHit hit;
        Debug.DrawLine(Camera.main.transform.position, target.position, Color.yellow, 1f);
        if (Physics.Raycast(Camera.main.transform.position, direction, out hit, direction.magnitude, obstructionLayers.value))
        {
            // Check if the hit object is not the target and is between the camera and the target
            if (hit.transform != target)
            {
                Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red, 1f);
                return true; // There is an obstruction
            }
        }

        return false; // No obstruction
    }
}
