using UnityEngine;

public class TurnManager
{
    public event System.Action OnTick;
    private int _turnCounter;

    public TurnManager()
    {
        _turnCounter = 1;
    }

    public void Tick()
    {
        OnTick?.Invoke();
        _turnCounter += 1;
        Debug.Log("Current turn count : " + _turnCounter);
    }
}
