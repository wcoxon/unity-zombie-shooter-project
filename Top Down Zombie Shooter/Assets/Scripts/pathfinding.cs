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
    //public GameObject HighlightMap;
    public GameObject WallMap;
    //public Tile RedHighlightTile;
    //public Tile GreenHighlightTile;
    //public Tile BlueHighlightTile;
    public Stack<Vector3> path;
    public GameObject target;
    public Vector3 targetpos;
    //Stack<Cell> path;
    public GameObject pf;
    //public Vector3 next;
    // Start is called before the first frame update
    void Start()
    {
        path = new Stack<Vector3>();
        pf = GameObject.Find("pathfinder");
        
        WallMap = GameObject.Find("Walls");

        target = GameObject.Find("player");

        path = pf.GetComponent<pathfindingfunction>().path(transform.position, target.transform.position, WallMap.GetComponent<Tilemap>());




    }
    /*public void updatenext()
    {
        next = GameObject.Find("pathfinder").GetComponent<pathfindingfunction>().nextTileTowards(transform.position, target.transform.position, WallMap.GetComponent<Tilemap>());
    }*/
    void OnEnable()
    {
        pf = GameObject.Find("pathfinder");
        WallMap = GameObject.Find("Walls");

        target = GameObject.Find("player");

        path = pf.GetComponent<pathfindingfunction>().path(transform.position, target.transform.position, WallMap.GetComponent<Tilemap>());

    }

    void FixedUpdate()
    {
        
        /*if(transform.position == next)
        {
            updatenext();
        }*/
        if(WallMap.GetComponent<Tilemap>().WorldToCell(target.transform.position) != WallMap.GetComponent<Tilemap>().WorldToCell(targetpos))
        {
            //Debug.Log("moved");
            targetpos = target.transform.position;
            path = pf.GetComponent<pathfindingfunction>().path(transform.position, target.transform.position, WallMap.GetComponent<Tilemap>());

        }
        if (transform.position == path.Peek()&&path.Count>1)
        {

            path.Pop();
        }
        transform.position = Vector3.MoveTowards(transform.position,path.Peek(), 5 * Time.deltaTime);

        

    }
}
