using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{

    public GameObject foodPrefab;


    public class CellData
    {
        public bool Passable;
        //public GameObject ContainedObject;
        public GameObject ContainedObject;
    }

    private CellData[,] _mBoarData; 
    private Tilemap m_tilemap;


    private Grid _mGrid;
    
    
    public int Height;
    public int Width;

    public Tile[] GroundTiles;

    public Tile[] WallTiles;

    
    
    
    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return _mGrid.GetCellCenterWorld((Vector3Int)cellIndex);
    }


    void GenerateFood()
    {
        int foodCount = 5;
        for (int i = 0; i < foodCount; ++i)
        {
            int randomX = Random.Range(1, Width-1);
            int randomY = Random.Range(1, Height-1);
            CellData data = _mBoarData[randomX, randomY];
            if (data.Passable && data.ContainedObject == null)
            {
                GameObject newFood = Instantiate(foodPrefab);
                newFood.transform.position = CellToWorld(new Vector2Int(randomX, randomY));
                data.ContainedObject = newFood;
            }
        }
    }

    public CellData GetCellData(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= Width || cellIndex.y < 0 || cellIndex.y >= Height)
        {
            return null;
        }

        return _mBoarData[cellIndex.x, cellIndex.y];
    }

    // Start is called before the first frame update
    public void Init()
    {
        _mGrid = GetComponentInChildren<Grid>();
        m_tilemap = GetComponentInChildren<Tilemap>();

        //Initialize the list
        
        
        _mBoarData = new CellData[Width, Height];

        for (int i = 0; i < Height; ++i)
        {
            for (int j = 0; j < Width; ++j)
            {
                Tile tile;
                _mBoarData[j, i] = new CellData();

                if (i == 0 || j == 0 || j == Width - 1 || i == Height - 1)
                {
                    tile = WallTiles[Random.Range(0, WallTiles.Length)];
                    _mBoarData[j, i].Passable = false;
                }
                else
                {
                    tile = GroundTiles[Random.Range(0, GroundTiles.Length)];
                    _mBoarData[j, i].Passable = true;
                    
                    //This is a passable empty cell, add it to the list!
                    
                }

                m_tilemap.SetTile(new Vector3Int(j, i, 0), tile);
            }
        }
        
        //remove the starting point of the player! It's not empty, the player is there

        GenerateFood();
        
    }

}