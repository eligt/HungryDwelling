using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HousePlacer : Singleton<HousePlacer>
{
    public GameObject placeProvider;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool isInsideAvailablePlaceOld(Vector2 position)
    {
        GameObject placeChild = placeProvider.transform.Find("Placer").gameObject;
        Renderer render = placeChild.GetComponent<Renderer>();
        Bounds bounds = render.bounds;

        Vector3 pos = new Vector3(
            position.x, 
            position.y, 
            bounds.center.z
        );

        return bounds.Contains(pos);
        //position = placeProvider.transform.InverseTransformPoint(position);

        //return false;
    }

    public bool isInsideAvailablePlace(Vector2 position)
    {
        MapControl map = placeProvider.GetComponent<MapControl>();

        return map.GetPlacerAtPosition(position);
    }

    public void placeHouseAt(HouseControl house, Vector2 position)
    {
        MapControl map = placeProvider.GetComponent<MapControl>();
        Vector3 placerPosition;
        float placerAngle;

        if (map.GetPlacerAtPosition(position, out placerPosition, out placerAngle))
        {
            Bounds houseBounds = house.GetBounds();
            house.transform.parent = placeProvider.transform;
            house.transform.position = new Vector3(placerPosition.x, placerPosition.y, -1.5f);
            //Debug.DrawLine(house.transform.position, house.transform.position + new Vector3(0.5f, 0f, 0f), Color.red);
            house.transform.rotation = Quaternion.identity;
            house.transform.rotation = Quaternion.AngleAxis(placerAngle, Vector3.forward);
            //house.transform.RotateAround(house.transform.position + houseBounds.extents, placerAngle);
            house.isPlaced = true;
        }
    }

    public void DestroyHouse(HouseControl house)
    {
        house.isPlaced = false;
        Destroy(house.gameObject);
    }

    public int GetPlacedCount()
    {
        HouseControl[] houses = placeProvider.GetComponentsInChildren<HouseControl>();
        int count = 0;

        foreach (HouseControl house in houses)
        {
            if (house.isPlaced)
                count++;
        }

        return count;
    }
}
