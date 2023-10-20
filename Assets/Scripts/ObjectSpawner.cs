using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab; // The prefab of the object to spawn
    public Tilemap tilemap; // The reference to the tilemap component
    public Camera cameraComponent; // The reference to the camera component

    public static ObjectSpawner instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SpawnObjectOutsideViewport();
    }

    public void SpawnObjectOutsideViewport()
    {
        // Get the bounds of the tilemap in world space
        Bounds tilemapBounds = tilemap.localBounds;

        // Calculate the camera's viewport bounds in world space
        float cameraHeight = 2f * cameraComponent.orthographicSize;
        float cameraWidth = cameraHeight * cameraComponent.aspect;
        Bounds viewportBounds = new Bounds(cameraComponent.transform.position, new Vector3(cameraWidth, cameraHeight, 0f));

        // Calculate the spawning area bounds (inside tilemap, outside viewport)
        Bounds spawningBounds = new Bounds(tilemapBounds.center, tilemapBounds.size);
        spawningBounds.Encapsulate(viewportBounds.min);
        spawningBounds.Encapsulate(viewportBounds.max);

        // Generate random position within the spawning area
        Vector3 randomPosition = new Vector3(
            Random.Range(spawningBounds.min.x, spawningBounds.max.x),
            Random.Range(spawningBounds.min.y, spawningBounds.max.y),
            0f);

        // Spawn the object at the random position
        Instantiate(objectPrefab, randomPosition, Quaternion.identity);
    }
}

