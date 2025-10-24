using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private BoardManager _mBoard;

    private Vector2Int _mCellPosition;

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        _mBoard = boardManager;
        _mCellPosition = cell;
        
        //lets move to the right position
        transform.position = _mBoard.CellToWorld(cell);
    }

    public void MoveTo(Vector2Int cell)
    {
        _mCellPosition = cell;
        transform.position = _mBoard.CellToWorld(_mCellPosition);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2Int newCellTarget = _mCellPosition;
        bool hasMoved = false;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y += 1;
            hasMoved = true;
        }

        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y -= 1;
            hasMoved = true;
        }

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x -= 1;
            hasMoved = true;
        }

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x += 1;
            hasMoved = true;
        }

        if(hasMoved)
        {
            //check if the new position is passable, then move there if it is.
            BoardManager.CellData cellData = _mBoard.GetCellData(newCellTarget);

            if(cellData != null && cellData.Passable)
            {
                GameManager.Instance.Turn.Tick();
                /*
                MoveTo(newCellTarget);

                if (cellData.ContainedObject != null)
                {
                    cellData.ContainedObject.PlayerEntered();
                }
                */

                if (cellData.ContainedObject == null)
                {
                    MoveTo(newCellTarget);
                }
                else if(cellData.ContainedObject.PlayerWantsToEnter())
                {
                    MoveTo(newCellTarget);
                    //call PlayEntered after moving the player! otherwise not in cell yet
                    cellData.ContainedObject.PlayerEntered();
                }
            }
        }
    }
}
