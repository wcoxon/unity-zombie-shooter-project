using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Cell
{
    /*
    public static List<Cell> Cells;
    public static List<Cell> ClosedCells;
    public static List<Cell> OpenCells;
    static Cell()
    {
        Cells = new List<Cell>();
        ClosedCells = new List<Cell>();
        OpenCells = new List<Cell>();
    }*/


    public int PathLength;

    public int DistanceFromTarget;
    public int FCost;
    public Vector3Int Coordinates;
    public Vector3Int TargetCoordinates;
    public Cell ParentCell;
    public Cell(Vector3Int coordinates, Vector3Int targetCoordinates, Cell parentCell)
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
        DistanceFromTarget = Mathf.RoundToInt(Vector3Int.Distance(coordinates, targetCoordinates) * 10);
        FCost = PathLength + DistanceFromTarget;
        //Debug.Log("test");
        //CompareCells(this, GetCellAtCoord(coordinates));

    }

    /*public static Cell GetCellAtCoord(Vector3Int coord)
    {
        for (int x = 0; x < Cells.Count; x++)
        {
            if (Cells[x].Coordinates == coord)
            {
                return Cells[x];
            }
        }
        return null;
    }*/
    /*public static void CompareCells(Cell cel1,Cell cel2)
    {
        if (cel2 == null)
        {
            Cells.Add(cel1);
            OpenCells.Add(cel1);
        }
        
        else if (OpenCells.Contains(cel2)&&(cel2.FCost > cel1.FCost || (cel2.FCost == cel1.FCost && cel2.DistanceFromTarget > cel1.DistanceFromTarget)))
        {
            OpenCells.Remove(cel2);
            Cells.Remove(cel2);
            Cells.Add(cel1);
            OpenCells.Add(cel1);
        }
        
    }*/

    /*public void select(Tilemap tm,Tilemap walls)
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
                else if (/*(x != 0 || y != 0) && *//*!walls.HasTile(Coordinates + new Vector3Int(x, y, 0)))
                {
                    new Cell(Coordinates + new Vector3Int(x, y, 0), TargetCoordinates, this);
                    

                }
                
            }
        }
    }*/

}

public class pathfinding : MonoBehaviour
{
    //public List<Cell> Cells;
   // public List<Cell> ClosedCells;
   // public List<Cell> OpenCells;
    public GameObject HighlightMap;
    public GameObject WallMap;
    //public Tile RedHighlightTile;
    //public Tile GreenHighlightTile;
    public Tile BlueHighlightTile;

    public GameObject target;
    //Stack<Cell> path;


    // Start is called before the first frame update
    void Start()
    {
        //Cells = new List<Cell>();
        //ClosedCells = new List<Cell>();
        //OpenCells = new List<Cell>();

        //path = new Stack<Cell>();
        //HighlightMap = GameObject.Find("HighlightMap");
        WallMap = GameObject.Find("Walls");

        target = GameObject.Find("player");



        //new Cell(HighlightMap.GetComponent<Tilemap>().WorldToCell(transform.position), HighlightMap.GetComponent<Tilemap>().WorldToCell(target.transform.position), null);
        //Cell.Cells[0].select(HighlightMap.GetComponent<Tilemap>(), WallMap.GetComponent<Tilemap>());
        //Debug.Log("test");
        //updatePath();
        //Debug.Log("test");

    }
    /*public Cell GetCellAtCoord(Vector3Int coord)
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
    public void CompareCells(Cell cel1, Cell cel2)
    {
        if (cel2 == null)
        {
            Cells.Add(cel1);
            OpenCells.Add(cel1);
        }

        else if (OpenCells.Contains(cel2) && (cel2.FCost > cel1.FCost || (cel2.FCost == cel1.FCost && cel2.DistanceFromTarget > cel1.DistanceFromTarget)))
        {
            OpenCells.Remove(cel2);
            Cells.Remove(cel2);
            Cells.Add(cel1);
            OpenCells.Add(cel1);
        }

    }
    public void select(Cell cel)
    {
        OpenCells.Remove(cel);
        ClosedCells.Add(cel);


        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (x * x == y * y && (WallMap.GetComponent<Tilemap>().HasTile(cel.Coordinates + new Vector3Int(x, 0, 0)) || WallMap.GetComponent<Tilemap>().HasTile(cel.Coordinates + new Vector3Int(0, y, 0))))
                {
                    continue;
                }
                else if (/*(x != 0 || y != 0) && *//*!WallMap.GetComponent<Tilemap>().HasTile(cel.Coordinates + new Vector3Int(x, y, 0)))
                {
                    //new Cell(cel.Coordinates + new Vector3Int(x, y, 0), cel.TargetCoordinates, cel);
                    CompareCells(new Cell(cel.Coordinates + new Vector3Int(x, y, 0), cel.TargetCoordinates, cel), GetCellAtCoord(cel.Coordinates + new Vector3Int(x, y, 0)));

                }

            }
        }
    }*/
    void OnEnable()
    {
        /*Cells = new List<Cell>();
        ClosedCells = new List<Cell>();
        OpenCells = new List<Cell>();
        path = new Stack<Cell>();*/
        //Debug.Log(path.Count);
        //HighlightMap = GameObject.Find("HighlightMap");
        WallMap = GameObject.Find("Walls");

        target = GameObject.Find("player");
        //updatePath();
    }
    /*void updatePath()
    {
        //Debug.Log("test");
        Cells.Clear();
        ClosedCells.Clear();
        OpenCells.Clear();

        CompareCells(new Cell(HighlightMap.GetComponent<Tilemap>().WorldToCell(transform.position), HighlightMap.GetComponent<Tilemap>().WorldToCell(target.transform.position), null), GetCellAtCoord(HighlightMap.GetComponent<Tilemap>().WorldToCell(transform.position)));

        select(Cells[0]);

        while (ClosedCells[ClosedCells.Count - 1].Coordinates != ClosedCells[ClosedCells.Count - 1].TargetCoordinates && ClosedCells.Count < 200)
        {

            OpenCells.Sort((x, y) => x.FCost.CompareTo(y.FCost));

            select(OpenCells[0]);
        }
        path.Clear();

        path.Push(GetCellAtCoord(Cells[0].TargetCoordinates));

        while (path.Peek().ParentCell != null && path.Count < 50)
        {
            path.Push(path.Peek().ParentCell);

        }

    }*/
    void FixedUpdate()
    {

        Vector3.MoveTowards(transform.position, GameObject.Find("pathfinder").GetComponent<pathfindingfunction>().nextTileTowards(transform.position, target.transform.position, WallMap.GetComponent<Tilemap>()),Time.deltaTime);
        HighlightMap.GetComponent<Tilemap>().SetTile(HighlightMap.GetComponent<Tilemap>().WorldToCell(GameObject.Find("pathfinder").GetComponent<pathfindingfunction>().nextTileTowards(transform.position, target.transform.position, WallMap.GetComponent<Tilemap>())), BlueHighlightTile);
        /*if (path.Count == 1)
        {
            Debug.Log("--------");
            Debug.Log(HighlightMap.GetComponent<Tilemap>().WorldToCell(target.transform.position));
            Debug.Log(Cells[0].TargetCoordinates);
        }
        
        if (HighlightMap.GetComponent<Tilemap>().WorldToCell(target.transform.position) != Cells[0].TargetCoordinates)
        {
            
            updatePath();

        }
        
        if (path.Count > 0)
        {
            if (path.Peek().Coordinates == HighlightMap.GetComponent<Tilemap>().WorldToCell(transform.position) && path.Count > 1)
            {
                path.Pop();
            }
            //HighlightMap.GetComponent<Tilemap>().SetTile(path.Peek().Coordinates, BlueHighlightTile);
            transform.position = Vector3.MoveTowards(transform.position, HighlightMap.GetComponent<Tilemap>().GetCellCenterWorld(path.Peek().Coordinates), 5 * Time.deltaTime);

        }*/

    }
}
