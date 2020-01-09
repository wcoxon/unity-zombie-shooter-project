using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class pathfindingfunction : MonoBehaviour
{

    // Start is called before the first frame update
    
    public GameObject player;
    
    public GameObject wall;
    void Start()
    {
        
        
    }
    public Cell GetCellAtCoord(Vector3Int coord,List<Cell> Cells)
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
    public void CompareCells(Cell cel1, Cell cel2,List<Cell> Cells,List<Cell> Open)
    {
        if (cel2 == null)
        {
            Cells.Add(cel1);
            Open.Add(cel1);
        }

        else if (Open.Contains(cel2) && (cel2.FCost > cel1.FCost || (cel2.FCost == cel1.FCost && cel2.DistanceFromTarget > cel1.DistanceFromTarget)))
        {
            Open.Remove(cel2);
            Cells.Remove(cel2);
            Cells.Add(cel1);
            Open.Add(cel1);
        }

    }
    
    public void select(Cell cel,List<Cell> Open,List<Cell> Closed,List<Cell> Cells,Tilemap walls)
    {
        Open.Remove(cel);
        Closed.Add(cel);


        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (x * x == y * y && (walls.HasTile(cel.Coordinates + new Vector3Int(x, 0, 0)) || walls.HasTile(cel.Coordinates + new Vector3Int(0, y, 0))))
                {
                    continue;
                }
                else if (!walls.HasTile(cel.Coordinates + new Vector3Int(x, y, 0)))
                {
                    
                    CompareCells(new Cell(cel.Coordinates + new Vector3Int(x, y, 0), cel.TargetCoordinates, cel), GetCellAtCoord(cel.Coordinates + new Vector3Int(x, y, 0),Cells),Cells,Open);

                }

            }
        }
    }
    public Vector3 nextTileTowards(Vector3 position, Vector3 target, Tilemap walls)
    {

        if (walls.HasTile(walls.WorldToCell(position)))
        {
            position = new Vector3(0, 0, 0);
        }
        if(walls.WorldToCell(position)== walls.WorldToCell(target))
        {
            return target;
        }
        List<Cell> Cells = new List<Cell>();
        List<Cell> Closed = new List<Cell>();
        List<Cell> Open = new List<Cell>();
        Stack<Cell> path = new Stack<Cell>();
        CompareCells(new Cell(walls.WorldToCell(position), walls.WorldToCell(target), null), GetCellAtCoord(walls.WorldToCell(position),Cells),Cells,Open);
        select(Cells[0],Open,Closed,Cells,walls);
        while (Open.Count > 0 && Closed[Closed.Count - 1].Coordinates != Closed[Closed.Count-1].TargetCoordinates)
        {
            Open.Sort((x, y) => x.FCost.CompareTo(y.FCost));

            select(Open[0],Open,Closed,Cells,walls);
        }
        path.Clear();

        path.Push(GetCellAtCoord(Cells[0].TargetCoordinates,Cells));

        while (path.Peek().ParentCell != null)
        {
            path.Push(path.Peek().ParentCell);

        }
        path.Pop();
        
        return walls.GetCellCenterWorld(path.Pop().Coordinates);

    }
    public Stack<Vector3> path(Vector3 position, Vector3 target, Tilemap walls)
    {
        if (walls.HasTile(walls.WorldToCell(position)))
        {
            position = new Vector3(0, 0, 0);
        }
        List<Cell> Cells = new List<Cell>();
        List<Cell> Closed = new List<Cell>();
        List<Cell> Open = new List<Cell>();
        Stack<Cell> path = new Stack<Cell>();
        Stack<Vector3> ret = new Stack<Vector3>();
        if (walls.WorldToCell(position) == walls.WorldToCell(target))
        {
            ret.Push(target);
            return ret;
        }
        CompareCells(new Cell(walls.WorldToCell(position), walls.WorldToCell(target), null), GetCellAtCoord(walls.WorldToCell(position), Cells), Cells, Open);
        select(Cells[0], Open, Closed, Cells, walls);
        while (Open.Count > 0 && Closed[Closed.Count - 1].Coordinates != Closed[Closed.Count - 1].TargetCoordinates)
        {
            Open.Sort((x, y) => x.FCost.CompareTo(y.FCost));

            select(Open[0], Open, Closed, Cells, walls);
        }
        path.Clear();

        path.Push(GetCellAtCoord(Cells[0].TargetCoordinates, Cells));

        while (path.Peek().ParentCell != null)
        {
            ret.Push(walls.GetCellCenterWorld(path.Peek().Coordinates));
            path.Push(path.Peek().ParentCell);

        }
        
        return ret;

    }
    // Update is called once per frame
    
}
