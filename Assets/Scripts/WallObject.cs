
using UnityEngine;

using UnityEngine.Tilemaps;


public class WallObject : CellObject
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Tile[] obstacleTile;
    public int maxHealth = 3;

    private int _mHealthPoint;
    private Tile _mOriginalTile;
    
    
    
    
    public override void Init(Vector2Int cell)
    {
        base.Init(cell);

        _mHealthPoint = maxHealth;

        _mOriginalTile = GameManager.Instance.board.GetCellTile(cell);

        int obstacleIndex = Random.Range(0, obstacleTile.Length);
        Tile newTile = obstacleTile[obstacleIndex];
        GameManager.Instance.board.SetCellTile(cell,newTile);   
    }

    public override bool PlayerWantsToEnter()
    {
        _mHealthPoint -= 1;
        if (_mHealthPoint > 0)
        {
            return false;
        }
        GameManager.Instance.board.SetCellTile(MCell,_mOriginalTile);
        Destroy(gameObject);
        return true;
    }
    
}
