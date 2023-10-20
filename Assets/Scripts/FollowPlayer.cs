using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FollowPlayer : MonoBehaviour
{
    public Transform target; // The target player object
    public Tilemap tilemap; // The reference to your tilemap component

    private Vector3 minBounds;
    private Vector3 maxBounds;

    private Camera cameraComponent;
    private float cameraHeight;
    private float cameraWidth;

    private bool isFollowingTarget = true;

    private void Start()
    {
        // Get the reference to the camera component
        cameraComponent = GetComponent<Camera>();

        // Calculate the bounds of the tilemap
        CalculateBounds();

        // Calculate the camera's dimensions
        cameraHeight = 2f * cameraComponent.orthographicSize;
        cameraWidth = cameraHeight * cameraComponent.aspect;
    }

    private void LateUpdate()
    {
        if (target != null && isFollowingTarget)
        {
            // Calculate the camera's position just before reaching the bounds
            Vector3 clampedPos = new Vector3(
                Mathf.Clamp(target.position.x, minBounds.x + cameraWidth / 2f, maxBounds.x - cameraWidth / 2f),
                Mathf.Clamp(target.position.y, minBounds.y + cameraHeight / 2f, maxBounds.y - cameraHeight / 2f),
                transform.position.z);

            // Check if the camera viewport reaches the bounds
            float cameraLeftBound = clampedPos.x - cameraWidth / 2f;
            float cameraRightBound = clampedPos.x + cameraWidth / 2f;
            float cameraBottomBound = clampedPos.y - cameraHeight / 2f;
            float cameraTopBound = clampedPos.y + cameraHeight / 2f;

            if (cameraLeftBound <= minBounds.x || cameraRightBound >= maxBounds.x ||
                cameraBottomBound <= minBounds.y || cameraTopBound >= maxBounds.y)
            {
                // Camera viewport reaches the bounds, stop following the player
                isFollowingTarget = false;
            }

            // Update the camera position
            transform.position = clampedPos;
        }
        else if (target != null && !isFollowingTarget)
        {
            // Check if the target player moves within the camera's viewport
            Vector3 viewportPos = cameraComponent.WorldToViewportPoint(target.position);

            if (viewportPos.x > 0f && viewportPos.x < 1f && viewportPos.y > 0f && viewportPos.y < 1f)
            {
                // Target is within the viewport, resume following
                isFollowingTarget = true;
            }
        }
    }

    private void CalculateBounds()
    {
        if (tilemap != null)
        {
            // Get the bounds of the tilemap in world space
            Bounds tilemapBounds = tilemap.localBounds;
            minBounds = tilemapBounds.min;
            maxBounds = tilemapBounds.max;
        }
    }
}
