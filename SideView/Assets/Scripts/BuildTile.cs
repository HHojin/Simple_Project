using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildTile : MonoBehaviour
{
    private Tilemap m_tileMap;
    private BoundsInt m_bounds;
    //private TileBase[] m_allTiles;
    //private TileBase m_tile;
    public TileBase m_possibleTile;

    void Start()
    {
        m_tileMap = GetComponent<Tilemap>();

        m_bounds = m_tileMap.cellBounds;

        /*
        m_allTiles = m_tileMap.GetTilesBlock(m_bounds);
        for (int x = 0; x < m_bounds.size.x; x++)
        {
            for (int y = 0; y < m_bounds.size.y; y++)
            {
                m_tile = m_allTiles[x + y * m_bounds.size.x];
                if (m_tile.name.Equals("dirt"))
                {
                    Debug.Log("x:" + x + " y:" + y + " tile:" + m_tile.name + "m_bound:" + (x+y*m_bounds.size.x));
                }
                else
                {
                    Debug.Log("x:" + x + " y:" + y + " tile:" + m_tile.name + "m_bound:" + (x+y*m_bounds.size.x));
                }
            }
        }
        */

        for (int x = m_bounds.xMin; x < m_bounds.xMax; x++)
        {
            for(int y = m_bounds.yMin; y < m_bounds.yMax; y++)
            {
                Debug.Log("x: " + x + " y: " + y);
                Vector3Int m_tilePos = new Vector3Int(x, y, 0);
                if(m_tileMap.GetTile(m_tilePos).name.Equals("empty"))
                {
                    Vector3Int[] m_tileDetect = new[] { new Vector3Int(x - 1, y, 0), new Vector3Int(x + 1, y, 0), new Vector3Int(x, y + 1, 0), new Vector3Int(x, y - 1, 0) };
                    for(int i = 0; i < m_tileDetect.Length; i++)
                    {
                        try
                        {
                            if (m_tileMap.GetTile(m_tileDetect[i]).name.Equals("dirt"))
                            {
                                m_tileMap.SetTile(m_tileDetect[i], m_possibleTile);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }
    }

    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 m_mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int m_gridPos = m_tileMap.WorldToCell(m_mousePos);

            if (m_tileMap.HasTile(m_gridPos))
            {
                Debug.Log("ASDFSAD" + m_gridPos);
            }

        }
        */
    }
}
