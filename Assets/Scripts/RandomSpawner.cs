using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner<T> : Singleton<T> where T : MonoBehaviour
{
    public GameObject spawnRoot;
    public GameObject spawnObject;

    public Bounds getCameraBounds()
    {
        Camera camera = Camera.main;
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

        return bounds;
    }

    public Bounds getSpawnBounds()
    {
        BoxCollider2D rootCollider = spawnRoot.GetComponent<BoxCollider2D>();
        return rootCollider.bounds;
    }

    public Vector2 getNextSpawnPoint()
    {
        PlayerControl player = GameObject.FindObjectsOfType(typeof(PlayerControl))[0] as PlayerControl;
        HouseControl house = player.getCurrentHouse();
        Bounds houseBounds = house.GetBounds();
        houseBounds.Expand(12.0f);
        Vector2 margins = new Vector2(5.0f, 5.0f);
        Bounds bounds = getSpawnBounds();
        Vector2 point;

        do
        {
            point = new Vector2(
                Random.Range(bounds.min.x + margins.x, bounds.max.x - margins.x),
                Random.Range(bounds.min.y + margins.y, bounds.max.y - margins.y)
            );
        }
        while (houseBounds.Contains(point));

        Vector3 relative = spawnRoot.transform.InverseTransformPoint(new Vector3(point.x, point.y, spawnRoot.transform.position.z));

        return new Vector2(relative.x, relative.y);
    }
}
