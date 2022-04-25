using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildTile : MonoBehaviour
{
    private Tilemap m_tileMap;
    private BoundsInt m_bounds;
    private TileBase[] m_allTiles;
    private TileBase m_tile;

    void Start()
    {
        m_tileMap = GetComponent<Tilemap>();

        m_bounds = m_tileMap.cellBounds;
        m_allTiles = m_tileMap.GetTilesBlock(m_bounds);
        Debug.Log(m_bounds.size.x);

        for (int x = 0; x < m_bounds.size.x; x++)
        {
            for (int y = 0; y < m_bounds.size.y; y++)
            {
                m_tile = m_allTiles[x + y * m_bounds.size.x];
                if (m_tile.name.Equals("dirt"))
                {
                    Debug.Log("x:" + x + " y:" + y + " tile:" + m_tile.name);
                }
                else
                {
                    Debug.Log("x:" + x + " y:" + y + " tile:" + m_tile.name);
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
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("UnderGround"))
                {
                    Vector3 m_mousePos = hit.point;
                    Vector3Int m_gridPos = m_tileMap.WorldToCell(m_mousePos);
                    Debug.Log(m_gridPos);
                }
            }
        }
        
    }
}
