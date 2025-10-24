using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    
    public WallObject wallPrefab;
        
    public FoodObject[] foodPrefab;

    
    private List<Vector2Int> _mEmptyCellsList;
    public class CellData
    {
        public bool Passable;
        //public GameObject ContainedObject;
        public CellObject ContainedObject;
    }

    private CellData[,] _mBoarData; 
    private Tilemap m_tilemap;


    private Grid _mGrid;
    
    
    public int Height;
    public int Width;

    public Tile[] GroundTiles;

    public Tile[] WallTiles;

    
    

    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        m_tilemap.SetTile(new Vector3Int(cellIndex.x,cellIndex.y,0),tile);
    }

    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return m_tilemap.GetTile<Tile>(new Vector3Int(cellIndex.x, cellIndex.y, 0));
    }
    
    
    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return _mGrid.GetCellCenterWorld((Vector3Int)cellIndex);
    }
    
    void AddObject(CellObject obj, Vector2Int coord)
    {
        CellData data = _mBoarData[coord.x, coord.y];
        obj.transform.position = CellToWorld(coord);
        data.ContainedObject = obj;
        obj.Init(coord);
    }

    void GenerateFood()
    {
        int foodCount = 5;
        for (int i = 0; i < foodCount; ++i)
        {
            int randomIndex = Random.Range(0, _mEmptyCellsList.Count);
            Vector2Int coord = _mEmptyCellsList[randomIndex];
            int randomIndexFood = Random.Range(0, foodPrefab.Length);

            _mEmptyCellsList.RemoveAt(randomIndex);
            FoodObject foodChoosed = foodPrefab[randomIndexFood];
            FoodObject newFood = Instantiate(foodChoosed);
            AddObject(newFood,coord);
        }
    }
    void GenerateWall()
    {
        int wallCount = Random.Range(6, 10);
        for (int i = 0; i < wallCount; ++i)
        {
            int randomIndex = Random.Range(0, _mEmptyCellsList.Count);
            Vector2Int coord = _mEmptyCellsList[randomIndex];
            
            _mEmptyCellsList.RemoveAt(randomIndex);
            WallObject newWall = Instantiate(wallPrefab);

            //init the wall
            /*
            newWall.Init(coord);
            
            newWall.transform.position = CellToWorld(coord);
            data.ContainedObject = newWall;
            */
            AddObject(newWall,coord);
            
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
        

        _mEmptyCellsList = new List<Vector2Int>();

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
                    _mEmptyCellsList.Add(new Vector2Int(j,i));
                }

                m_tilemap.SetTile(new Vector3Int(j, i, 0), tile);
            }
        }
        
        //remove the starting point of the player! It's not empty, the player is there
        _mEmptyCellsList.Remove(new Vector2Int(1, 1));
        
        
        GenerateWall();
        GenerateFood();
        
    }

}