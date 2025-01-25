
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance{ get; private set;}

    private Grid _grid;

    public Grid Grid{
        get{ return _grid; }
        private set{ _grid = value; }
    }

    
    public float GridSize => _grid.cellSize.x;

    private void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(Instance);
        _grid = GetComponent<Grid>();
    }

    public Grid GetGrid()
    {
        return _grid;
    }

    
}