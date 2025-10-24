using UnityEngine;

public class CellObject : MonoBehaviour
{

    protected Vector2Int MCell;

    public virtual bool PlayerWantsToEnter()
    {
        return true;
    }
    public virtual void Init(Vector2Int cell)
    {
        MCell = cell; 
    }
 
    public virtual void PlayerEntered()
    {

    }
}
