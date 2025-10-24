using System.Security.Cryptography;
using UnityEngine.UIElements;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public BoardManager board;
    public PlayerController player;

    public TurnManager Turn { get; private set;}

    private int _foodAmount = 100;

    public UIDocument uiDoc;

    private Label _mFoodLabel;
    

    void OnTurnHappen()
    {
        ChangeFood(-1);
    }

    public void ChangeFood(int amount)
    {
        _foodAmount += amount;
        _mFoodLabel.text = "Food : " + _foodAmount;
    }    
    
    private void Awake()
   
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mFoodLabel = uiDoc.rootVisualElement.Q<Label>("FoodLabel");
        
        Turn = new TurnManager();
        Turn.OnTick += OnTurnHappen;
        
        board.Init();
        player.Spawn(board,new Vector2Int(1,1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
