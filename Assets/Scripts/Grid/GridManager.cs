
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance{ get; private set;}

    private Grid _grid;

    private void Start()
    {
        _grid = GetComponent<Grid>();
    }
    
    public float GridSize => _grid.cellSize.x;

    private void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(Instance);
    }

    public Grid GetGrid()
    {
        return _grid;
    }

    
}