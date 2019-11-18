using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cell
{
    public static List<Cell> Cells;
    public static List<Cell> ClosedCells;
    public static List<Cell> OpenCells;
    static Cell()
    {
        Cells = new List<Cell>();
        ClosedCells = new List<Cell>();
        OpenCells = new List<Cell>();
    }

    
    public int PathLength;
    
    public int DistanceFromTarget;
    public int FCost;
    public Vector3Int Coordinates;
    public Vector3Int TargetCoordinates;
    public Cell ParentCell;
    public Cell(Vector3Int coordinates,Vector3Int targetCoordinates,Cell parentCell)
    {
        
        if (parentCell == null)
        {
            PathLength = 0;
        }
        else
        {

            PathLength = parentCell.PathLength + Mathf.RoundToInt(Vector3Int.Distance(coordinates, parentCell.Coordinates) * 10);
            ParentCell = parentCell;
        }
        
        Coordinates = coordinates;
        TargetCoordinates = targetCoordinates;
        DistanceFromTarget = Mathf.RoundToInt(Vector3Int.Distance(coordinates, targetCoordinates)*10);
        FCost = PathLength + DistanceFromTarget;
        
        if (ShouldReplace(this))
        {
            Cells.Add(this);
            OpenCells.Add(this);
        }
    }
    
    public static Cell GetCellAtCoord(Vector3Int coord)
    {
        for (int x = 0; x < Cells.Count; x++)
        {
            if (Cells[x].Coordinates == coord)
            {
                return Cells[x];
            }
        }
        return null;
    }
    public static bool ShouldReplace(Cell newcell)
    {
        if (GetCellAtCoord(newcell.Coordinates)!=null)
        {
            
            if (GetCellAtCoord(newcell.Coordinates).FCost > newcell.FCost || (GetCellAtCoord(newcell.Coordinates).FCost == newcell.FCost && GetCellAtCoord(newcell.Coordinates).DistanceFromTarget > newcell.DistanceFromTarget))
            {
                Cells.Remove(GetCellAtCoord(newcell.Coordinates));
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            
            return true;
        }
        
    }
    
    public void select(Tilemap tm,Tilemap walls, Tile redTile,Tile greenTile)
    {
        OpenCells.Remove(this);
        ClosedCells.Add(this);

        
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if(x*x==y*y&& (walls.HasTile(Coordinates + new Vector3Int(x,0, 0))|| walls.HasTile(Coordinates + new Vector3Int(0, y, 0)))){
                    continue;
                }
                else if ((x != 0 || y != 0) && !walls.HasTile(Coordinates + new Vector3Int(x, y, 0)))
                {
                    new Cell(Coordinates + new Vector3Int(x, y, 0), TargetCoordinates, this);
                    

                }
                
            }
        }
    }
    
}

public class pathfinding : MonoBehaviour
{
    public GameObject HighlightMap;
    public GameObject WallMap;
    public Tile RedHighlightTile;
    public Tile GreenHighlightTile;
    public Tile BlueHighlightTile;
    
    public GameObject target;
    public Stack<Cell> path;
    
    
    // Start is called before the first frame update
    void Start()
    {
        HighlightMap = GameObject.Find("HighlightMap");
        WallMap = GameObject.Find("Walls");
        //HighlightMap = Instantiate(GameObject.Find("HighlightMap"),new Vector3(0,0,0),Quaternion.Euler(0,0,0));
        //HighlightMap.transform.SetParent(GameObject.Find("Grid").transform);
        //WallMap = GameObject.Find("Walls");
        target = GameObject.Find("player");

        //locked = new List<Cell>();
        //Debug.Log(Cell.Cells.Count);
        new Cell(HighlightMap.GetComponent<Tilemap>().WorldToCell(transform.position), HighlightMap.GetComponent<Tilemap>().WorldToCell(target.transform.position), null);

        
        Cell.Cells[0].select(HighlightMap.GetComponent<Tilemap>(), WallMap.GetComponent<Tilemap>(), RedHighlightTile, GreenHighlightTile);
        

    }
    
    void Update()
    {
        //target.transform.Translate(Vector3.right *Input.GetAxisRaw("Horizontal")* Time.deltaTime+ Vector3.up * Input.GetAxisRaw("Vertical") * Time.deltaTime);
        //Vector3.MoveTowards(transform.position, path.Peek().Coordinates, 5 * Time.deltaTime);
        
        Cell.Cells.Clear();
        Cell.ClosedCells.Clear();
        Cell.OpenCells.Clear();

        new Cell(HighlightMap.GetComponent<Tilemap>().WorldToCell(transform.position), HighlightMap.GetComponent<Tilemap>().WorldToCell(target.transform.position), null);
        Cell.Cells[0].select(HighlightMap.GetComponent<Tilemap>(), WallMap.GetComponent<Tilemap>(), RedHighlightTile, GreenHighlightTile);

        while (Cell.ClosedCells[Cell.ClosedCells.Count - 1].Coordinates != Cell.ClosedCells[Cell.ClosedCells.Count - 1].TargetCoordinates && Cell.ClosedCells.Count < 200)
        {
            Cell.OpenCells.Sort((x, y) => x.FCost.CompareTo(y.FCost));
            Cell.OpenCells[0].select(HighlightMap.GetComponent<Tilemap>(), WallMap.GetComponent<Tilemap>(), RedHighlightTile, GreenHighlightTile);
        }
        path = new Stack<Cell>();
        path.Push(Cell.GetCellAtCoord(Cell.Cells[0].TargetCoordinates));
        while (path.Peek().ParentCell.ParentCell != null && path.Count < 50)
        {
            path.Push(path.Peek().ParentCell);
        }

        HighlightMap.GetComponent<Tilemap>().ClearAllTiles();
        
        

        foreach (Cell cel in Cell.Cells)
        {
            /*if (path.Contains(cel))
            {
                HighlightMap.GetComponent<Tilemap>().SetTile(cel.Coordinates, BlueHighlightTile);
            }*/
            /*else if (Cell.ClosedCells.Contains(cel))
            {
                HighlightMap.GetComponent<Tilemap>().SetTile(cel.Coordinates, RedHighlightTile);
            }
            else if (Cell.OpenCells.Contains(cel))
            {
                HighlightMap.GetComponent<Tilemap>().SetTile(cel.Coordinates, GreenHighlightTile);
            }*/
            

        }
        if (Input.GetMouseButtonDown(0) && Cell.GetCellAtCoord(HighlightMap.GetComponent<Tilemap>().WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)))!=null)
        {
            Debug.Log(Cell.GetCellAtCoord(HighlightMap.GetComponent<Tilemap>().WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition))).PathLength);
            Debug.Log(Cell.GetCellAtCoord(HighlightMap.GetComponent<Tilemap>().WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition))).DistanceFromTarget);
            Debug.Log(Cell.GetCellAtCoord(HighlightMap.GetComponent<Tilemap>().WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition))).FCost);
            
            Debug.Log(Cell.GetCellAtCoord(HighlightMap.GetComponent<Tilemap>().WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition))).ParentCell.Coordinates);
            
            
        }
        if (Input.GetMouseButton(1))
        {
            Debug.Log(Cell.Cells.Count);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        if (path.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, path.Peek().Coordinates, 5 * Time.deltaTime);
        }

    }
}
