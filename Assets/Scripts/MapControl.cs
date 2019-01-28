using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapControl : MonoBehaviour
{

    protected Transform GetTerrain()
    {
        Transform terrain = transform.Find("Terrain");
        return terrain;
    }

    public Vector3Int GetTileIndexAtPosition(Vector2 position)
    {
        Transform terrain = GetTerrain();
        Tilemap tileMap = terrain.GetComponent<Tilemap>();
        Vector3 pos2 = new Vector3(position.x, position.y, terrain.position.z);
        Vector3Int pos = tileMap.WorldToCell(position);
        return pos;
    }

    public TileBase GetTileAtPosition(Vector2 position)
    {
        Transform terrain = GetTerrain();
        Tilemap tileMap = terrain.GetComponent<Tilemap>();
        Vector3Int pos = GetTileIndexAtPosition(position);
        return tileMap.GetTile(pos);
    }

    public bool GetPlacerAtPosition(Vector2 position)
    {
        Vector3 outPos;
        float angle;

        return GetPlacerAtPosition(position, out outPos, out angle);
    }

    public bool GetPlacerAtPosition(Vector2 position, out Vector3 newPosition, out float angle)
    {
        Transform terrain = GetTerrain();
        Tilemap tileMap = terrain.GetComponent<Tilemap>();
        Vector3Int pos = GetTileIndexAtPosition(position);
        TileBase tile = tileMap.GetTile(pos);

        if (tile != null)
        {
            TileData tileData = new TileData();

            tile.GetTileData(pos, null, ref tileData);

            if (tileData.sprite.name.StartsWith("Foundation_"))
            {
                Vector3 boundsSize = tileData.sprite.bounds.size;
                newPosition = tileMap.CellToWorld(pos);
                Vector3 adjust = new Vector3(boundsSize.x * 1.25f, boundsSize.y * 0.75f, 1.0f);
                angle = 0.0f;

                Vector3Int newPos;

                if (angle == 0.0f)
                {
                    newPos = pos + new Vector3Int(0, +1, 0);
                    tile = tileMap.GetTile(newPos);

                    if (tile != null)
                    {
                        tile.GetTileData(newPos, null, ref tileData);

                        if (tileData.sprite.name.StartsWith("Road_"))
                        {
                            angle = 90.0f;
                            adjust = new Vector3(boundsSize.x * 0.75f, boundsSize.y * 1.25f, 0.0f);
                        }
                    }
                }
                
                if (angle == 0.0f)
                {
                    newPos = pos + new Vector3Int(-1, 0, 0);
                    tile = tileMap.GetTile(newPos);

                    if (tile != null)
                    {
                        tile.GetTileData(newPos, null, ref tileData);

                        if (tileData.sprite.name.StartsWith("Road_"))
                        {
                            angle = 180.0f;
                            adjust = new Vector3(boundsSize.x * 0.25f, boundsSize.y * 0.75f, 0.0f);
                        }
                    }
                }

                if (angle == 0.0f)
                {
                    newPos = pos + new Vector3Int(0, -1, 0);
                    tile = tileMap.GetTile(newPos);

                    if (tile != null)
                    {
                        tile.GetTileData(newPos, null, ref tileData);

                        if (tileData.sprite.name.StartsWith("Road_"))
                        {
                            angle = 180.0f + 90.0f;
                            adjust = new Vector3(boundsSize.x * 0.75f, boundsSize.y * 0.25f, 0.0f);
                        }
                    }
                }

                newPosition += adjust;

                return true;
            }
        }

        angle = 0.0f;
        newPosition = Vector3.zero;
        return false;
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
