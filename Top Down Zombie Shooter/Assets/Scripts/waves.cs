using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class waves : MonoBehaviour
{
    public int wave;
    //public float ammoCounter;
    //public GameObject ammo;
    //public GameObject pf;
    public UnityEngine.Tilemaps.Tilemap Zone;
    public UnityEngine.Tilemaps.Tilemap Highlighter;
    public UnityEngine.Tilemaps.Tile HighlighterTile;
    //public UnityEngine.Tilemaps.Tile HighlighterTilered;
    public pathfindingfunction pf;
    public UnityEngine.Tilemaps.Tilemap WallMap;
    public UnityEngine.Tilemaps.TilemapCollider2D WallCol;
    public playerscript ps;
    public GameObject zombie;
    //public GameObject spawners;
    public Stack<GameObject> pool;
    public UnityEngine.UI.Text WaveIndicator;
    public UnityEngine.UI.Text EnterPrompt;
    Vector2 cameraDimensions;
    // Start is called before the first frame update
    void Start()
    {
        cameraDimensions = new Vector2( 2 * Camera.main.orthographicSize * Screen.width / Screen.height,2*Camera.main.orthographicSize);
        //ammoCounter = 0;
        EnterPrompt.rectTransform.anchoredPosition = 250 * Vector3.down;
        pool = new Stack<GameObject>();
    }
    public void clearZombies()
    {
        foreach (Transform child in transform)///////////
        {
            Destroy(child.gameObject);
        }
    }
    public Vector3Int squareAroundPlayer(Vector2 dimensions)
    {
        //dimensions.x-=
        List<Vector3Int> cells = new List<Vector3Int>();
        //Rect rec = new Rect(Camera.main.transform.position, new Vector2(2*Camera.main.orthographicSize, 2*Camera.main.orthographicSize*Screen.width/Screen.height));
        //Rect rec = new Rect(ps.transform.position-(Vector3)dimensions/2, dimensions);
        //GameObject gam = GameObject.Find("GameObject");
        //gam.transform.localScale = dimensions;
        //gam.transform.position = ps.transform.position;
        //dimensions += 2*Vector2.right + 2*Vector2.up;
        //Camera.main.rect.Contains
        //Vector3Int BL = Zone.WorldToCell(ps.transform.position - (Vector3)dimensions / 2);
        //Vector3Int TR = Zone.WorldToCell(ps.transform.position + (Vector3)dimensions / 2);
        Vector3Int BL = Zone.WorldToCell(Camera.main.transform.position - (Vector3)dimensions / 2);
        Vector3Int TR = Zone.WorldToCell(Camera.main.transform.position + (Vector3)dimensions / 2);

        for (int y = BL.y; y <= TR.y; y++)
        {
            for (int x = BL.x; x <= TR.x; x++)
            {
                if ((BL.x == x||BL.y==y||TR.x==x||TR.y==y)&&!WallMap.HasTile(Vector3Int.right * x + Vector3Int.up * y) && Zone.HasTile(Vector3Int.right * x + Vector3Int.up * y))
                {
                    //Highlighter.SetTile(Vector3Int.up * y + Vector3Int.right * x, HighlighterTile);//for testing
                    cells.Add(Vector3Int.up * y + Vector3Int.right * x);
                }
            }
        }
        return cells[Random.Range(0,cells.Count)];
        /*foreach(Tile til in Highlighter.GetTilesBlock(new BoundsInt(Vector3Int.FloorToInt(ps.transform.position), Vector3Int.RoundToInt(dimensions))))
        {
            
        }*/

        //Highlighter.SetTilesBlock(new BoundsInt(Vector3Int.FloorToInt(ps.transform.position), Vector3Int.RoundToInt(dimensions)), HighlighterTile);
        /*for (int y = Highlighter.WorldToCell(ps.transform.position-Vector3.up*dimensions.y/2).y;y<= Highlighter.WorldToCell(ps.transform.position + Vector3.up * dimensions.y / 2).y; y++)
        {
            for (int x = Highlighter.WorldToCell(ps.transform.position - Vector3.right * dimensions.x / 2).x; x <= Highlighter.WorldToCell(ps.transform.position + Vector3.right * dimensions.x / 2).x; x++)
            {
                if (WallMap.HasTile(Vector3Int.right * x + Vector3Int.up * y)||!Zone.HasTile(Vector3Int.right * x + Vector3Int.up * y))
                {

                    Highlighter.SetTile(Vector3Int.right * x + Vector3Int.up * y, HighlighterTilered);
                    //continue;
                }
                else
                {
                    Highlighter.SetTile(Vector3Int.right * x + Vector3Int.up * y, HighlighterTile);
                }
            }
        }*/

    }
    /*public Vector3Int spawnpoint(Tilemap zone, Tilemap walls, Camera camera)
    {
        
        return new Vector3Int(0,0,0);
    }*/
    /*public void spawnzombies(int number)
    {
        
        int instantiate = Mathf.Max(number - pool.Count,0);
        int reuse = Mathf.Min(pool.Count, number);
        //Debug.Log("zombies " + number);
        //Debug.Log("instantiate " + (number - pool.Count) + " reuse " + pool.Count);
        //Debug.Log("reuse " + pool.Count);
        for (int x = 0;x<instantiate; x++)
        {
            Instantiate(zombie, spawners.transform.GetChild(Random.Range(0, spawners.transform.childCount)).transform.position+Random.Range(-.25f,.25f)*new Vector3(1,1,0),Quaternion.Euler(0,0,0),transform);

        }
        for(int x = 0; x<reuse; x++)
        {
            pool.Peek().transform.localScale = new Vector3(1, 1, 1);
            pool.Peek().transform.SetParent(transform);
            pool.Peek().transform.position = spawners.transform.GetChild(Random.Range(0, spawners.transform.childCount)).transform.position + Random.Range(-.25f, .25f) * new Vector3(1, 1, 0);
            pool.Peek().GetComponent<CircleCollider2D>().enabled = true;
            pool.Pop().GetComponent<pathfinding>().enabled = true;
            //pool.Pop().SetActive(true);

        }
    }*/
    /*public Vector3 randomEmpty()
    {
        Vector3Int x;
        for (x = new Vector3Int(Random.Range(-16, 16), Random.Range(-16, 16), 0); WallMap.HasTile(x); x = new Vector3Int(Random.Range(-16, 16), Random.Range(-16, 16), 0));
        return Zone.GetCellCenterWorld(x);
        
    }*/
    public IEnumerator SpawnZombies(int number,float interval)
    {
        cameraDimensions = new Vector2(2 * Camera.main.orthographicSize * Screen.width / Screen.height, 2 * Camera.main.orthographicSize);
        //squareAroundPlayer(new Vector2(3, 3));
        //yield break;
        int instantiate = Mathf.Max(number - pool.Count, 0);
        int reuse = Mathf.Min(pool.Count, number);

        //List<Vector3Int> cells = new List<Vector3Int>();
        //Debug.Log("zombies " + number);
        //Debug.Log("instantiate " + (number - pool.Count) + " reuse " + pool.Count);
        //Debug.Log("reuse " + pool.Count);
        for (int x = 0; x < instantiate; x++)
        {
            //cells = squareAroundPlayer(new Vector2(12, 8));
            //Instantiate(zombie, spawners.transform.GetChild(Random.Range(0, spawners.transform.childCount)).transform.position + Random.Range(-.25f, .25f) * new Vector3(1, 1, 0), Quaternion.Euler(0, 0, 0), transform);
            //Instantiate(zombie, squareAroundPlayer(new Vector2(12, 8)), Quaternion.Euler(0, 0, 0), transform);
            Instantiate(zombie, squareAroundPlayer(cameraDimensions+new Vector2(2,2)), Quaternion.Euler(0, 0, 0), transform);
            yield return new WaitForSeconds(interval);
        }
        for (int x = 0; x < reuse; x++)
        {
            pool.Peek().transform.localScale = new Vector3(1, 1, 1);
            pool.Peek().transform.SetParent(transform);
            //pool.Peek().transform.position = spawners.transform.GetChild(Random.Range(0, spawners.transform.childCount)).transform.position + Random.Range(-.25f, .25f) * new Vector3(1, 1, 0);
            pool.Peek().transform.position = squareAroundPlayer(cameraDimensions + new Vector2(2, 2));
            pool.Peek().GetComponent<CircleCollider2D>().enabled = true;
            pool.Pop().GetComponent<pathfinding>().enabled = true;
            yield return new WaitForSeconds(interval);
            //pool.Pop().SetActive(true);

        }
        yield break;
    }
    public void incrementWave()
    {
        if (pool.Count == transform.childCount)
        {
            //EnterPrompt.rectTransform.anchoredPosition = new Vector3(0,-300,0);
            //EnterPrompt.enabled = false;
            //Debug.Log("new wave");
            wave += 1;
            StartCoroutine(SpawnZombies(wave, 0.5f));
            //spawnzombies(wave);
        }
    }
    // Update is called once per frame
    void Update()
    {
        /*ammoCounter += Time.deltaTime;
        if (ammoCounter >= 0.5)
        {
            Instantiate(ammo, randomEmpty(), Quaternion.Euler(0, 0, 0));
            ammoCounter = 0;
        }*/
        /*if (Input.GetKeyDown(KeyCode.Return))
        {
            Instantiate(ammo, randomEmpty(),Quaternion.Euler(0,0,0));
        }*/
        //Highlighter.ClearAllTiles();
        //squareAroundPlayer(cameraDimensions + new Vector2(2, 2));
        //GameObject.Find("wave").GetComponent<UnityEngine.UI.Text>().text = "wave: " + wave;
        WaveIndicator.text = "wave: " + wave;
        if (pool.Count == transform.childCount)
        {
            EnterPrompt.enabled = true;
            /*if (Mathf.FloorToInt(EnterPrompt.rectTransform.anchoredPosition.y) != -250)
            {*/
            //Debug.Log(EnterPrompt.rectTransform.anchoredPosition.y);
            //EnterPrompt.rectTransform.position += new Vector3(0, (-100 - EnterPrompt.rectTransform.position.y) / 3, 0);
            //EnterPrompt.rectTransform.anchoredPosition = new Vector3(0, EnterPrompt.rectTransform.anchoredPosition.y + (-100 - EnterPrompt.rectTransform.anchoredPosition.y) / 3, 0);
            //}
            //Debug.Log("new wave");
            //wave += 1;
            //StartCoroutine(Spawnz(wave,0.5f));
            //spawnzombies(wave);
        }
        else {
            //EnterPrompt.rectTransform.anchoredPosition = new Vector3(0, EnterPrompt.rectTransform.anchoredPosition.y+(-250 - EnterPrompt.rectTransform.anchoredPosition.y) /3, 0);
            /*if (Mathf.FloorToInt(EnterPrompt.rectTransform.anchoredPosition.y) != -350)
            {*/
            //EnterPrompt.rectTransform.position += new Vector3(0, (-350 - EnterPrompt.rectTransform.anchoredPosition.y) / 3, 0);
            //}
        }
    }
}
