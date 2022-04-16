using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int m_width;
    private int m_height;
    private int[,] m_gridArray;

    public Grid(int width, int height)
    {
        m_width = width;
        m_height = height;

        m_gridArray = new int[m_width, m_height];

        Debug.Log(m_width + " " + m_height + " ");

        for(int x = 0; x < m_gridArray.GetLength(0); x++)
        {
            for(int y = 0; y < m_gridArray.GetLength(1); y++)
            {
                //????
            }
        }
    }
}
