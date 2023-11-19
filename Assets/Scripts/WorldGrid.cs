using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrid
{
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridArray;

    public WorldGrid(int width, int height, float cellSize, float gridStartX, float gridStartY)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width, height];

        int tempX = Mathf.FloorToInt(gridStartX + gridArray.GetLength(0));
        int tempY = Mathf.FloorToInt(gridStartY + gridArray.GetLength(1));

        for (int x = Mathf.FloorToInt(gridStartX); x < tempX; x++)
        {
            for (int y = Mathf.FloorToInt(gridStartY); y < tempY; y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
    }

    public Vector2 GetWorldToCell(Vector2 pos)
    {
        int x = Mathf.FloorToInt(pos.x);
        int y = Mathf.FloorToInt(pos.y);

        return new Vector2(x, y) * cellSize;
    }
    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(x, y) * cellSize;
    }
    public Vector2 GetCellCentre(Vector2 cellPos)
    {
        Vector2 temp;
        temp = cellPos + (new Vector2(cellSize, cellSize) * 0.5f);

        return temp;
    }
}
