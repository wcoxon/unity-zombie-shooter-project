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
    //public GameObject WallMap;
    
    Tilemap WallMap;
    //TilemapCollider2D WallCol;
    //public Tile RedHighlightTile;
    //public Tile GreenHighlightTile;
    //public Tile BlueHighlightTile;
    public Stack<Vector3> path;
    public GameObject target;
    public Vector3 targetpos;
    public Rigidbody2D rb;
    //Stack<Cell> path;
    //public GameObject pf;
    //pathfindingfunction pf;
    //public Vector3 next;
    // Start is called before the first frame update
    public zombiescript zs;
    float attacktimer;
    
    //Vector2 velocity;
    /*void Start()
    {
        path = new Stack<Vector3>();
        //pf = GameObject.Find("pathfinder");
        pf = GetComponent<zombiescript>().WaveScript.pf;

        //WallMap = GameObject.Find("Walls");
        WallMap = GetComponent<zombiescript>().WaveScript.WallMap;

        target = GameObject.Find("player");

        path = pf.GetComponent<pathfindingfunction>().path(transform.position, target.transform.position, WallMap.GetComponent<Tilemap>());




    }*/

    /*public void updatenext()
    {
        next = GameObject.Find("pathfinder").GetComponent<pathfindingfunction>().nextTileTowards(transform.position, target.transform.position, WallMap.GetComponent<Tilemap>());
    }*/
    float normalise(float x, float y)
    {
        if (x == 0)
        {
            return 0;
        }
        return x / Mathf.Pow(x * x + y * y, 0.5f);
    }
    public Vector2 normalise(Vector2 vector)
    {
        return new Vector2(normalise(vector.x, vector.y), normalise(vector.y, vector.x));
    }
    void OnEnable()
    {
        attacktimer = 0;
        // rb = GetComponent<Rigidbody2D>();
        //pf = GameObject.Find("pathfinder");
        //Debug.Log(GetComponent<zombiescript>().WaveScript.wave);
        //zs.WaveScript.pf
        //pf = GetComponent<zombiescript>().WaveScript.pf;

        //WallMap = GameObject.Find("Walls");
        //velocity = new Vector2(0, 0);
        //WallCol = GetComponent<zombiescript>().WaveScript.WallCol;
        WallMap = GetComponent<zombiescript>().WaveScript.WallMap;

        target = GameObject.Find("player");

        path = zs.WaveScript.pf.GetComponent<pathfindingfunction>().path(transform.position, target.transform.position, WallMap.GetComponent<Tilemap>());

    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("collided");
        if (other.gameObject.tag == "Zombie"&&!WallMap.HasTile(WallMap.WorldToCell(transform.position)))
        {
            //Debug.Log("collided");
            transform.position -= (Vector3)normalise((Vector2)other.transform.position - (Vector2)transform.position)*(1-Vector3.Magnitude(other.transform.position- transform.position));
        }
    }
    void FixedUpdate()
    {

        /*if (zs.coll.IsTouching(WallCol))
        {
            //zs.coll.
            Debug.Log("test");

        }*/
        /*if(transform.position == next)
        {
            updatenext();
        }*/
        //when player position changes update the path
        if(WallMap.WorldToCell(target.transform.position) != WallMap.WorldToCell(targetpos)&& !WallMap.HasTile(WallMap.WorldToCell(transform.position)))
        {
            //Debug.Log("moved");
            targetpos = target.transform.position;
            path = zs.WaveScript.pf.path(transform.position, target.transform.position, WallMap);
        }
        //when you stand on a node, remove it and focus on the next node in the stack, (unless there are no more nodes in the stack after the current one)
        if (WallMap.WorldToCell(transform.position) == WallMap.WorldToCell(path.Peek())&&path.Count>1)
        {
            path.Pop();
        }
        //rb.MovePosition(Vector3.MoveTowards(transform.position, path.Peek(), 5 * Time.deltaTime));
        //transform.position = Vector3.MoveTowards(transform.position,path.Peek(), 5 * Time.deltaTime);
        //velocity = normalise(path.Peek() - transform.position)*5;
        //rb.MovePosition(Vector3.MoveTowards(transform.position, path.Peek(), 5 * Time.deltaTime));

        //rb.velocity += normalise(path.Peek() - transform.position) * 50 * Time.fixedDeltaTime;

        //rb.velocity = normalise(rb.velocity) * Mathf.Min(rb.velocity.magnitude, 5);

        //rb.AddForce(normalise(path.Peek() - transform.position)*20);
        //accelerate
        if (rb.velocity.magnitude > 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(rb.velocity.y, rb.velocity.x) * 180 / Mathf.PI - 90);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(target.transform.position.y-transform.position.y, target.transform.position.x - transform.position.x) * 180 / Mathf.PI - 90);
        }
        if (Vector3.SqrMagnitude(transform.position - target.transform.position) > 2)
        {
            attacktimer = 0;

            rb.velocity += normalise(path.Peek() - transform.position) * 50 * Time.fixedDeltaTime;

            //rb.velocity += rb.velocity * -0.25f;
            //limit velocity to 5
            rb.velocity = normalise(rb.velocity) * Mathf.Min(rb.velocity.magnitude, 5);
            //rb.AddForce(rb.velocity * -0.25f);
        }
        else
        {
            attacktimer += Time.fixedDeltaTime;
            if (attacktimer >= 0.75f)
            {
                zs.WaveScript.ps.health -= 35;
                zs.WaveScript.ps.Healthbar.anchorMax = new Vector2(zs.WaveScript.ps.health / 100,1);
                zs.WaveScript.ps.HealthIndicator.text = zs.WaveScript.ps.health + " HP";
                zs.animator.SetTrigger("Attack");
                attacktimer = 0;
            }
        }
        //rb.velocity = normalise(path.Peek() - transform.position)*5;



    }
}
