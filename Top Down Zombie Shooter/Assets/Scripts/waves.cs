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
    public guns gs;
    public GameObject zombie;
    //public GameObject spawners;
    public Stack<GameObject> pool;
    public UnityEngine.UI.Text WaveIndicator;
    public UnityEngine.UI.Text EnterPrompt;
    public UnityEngine.UI.Text upgradeMessage;
    public Vector2 cameraDimensions;
    public GameObject upgrader;

    public int points;
    //public int pointLimit;
    public UnityEngine.UI.Text PointsUI;
    //public RectTransform PointsBar;

    public float upgradeCounter;
    IEnumerator IndefiniteSpawner;
    //bool wavebreak;
    // Start is called before the first frame update
    void Start()
    {
        cameraDimensions = new Vector2( 2 * Camera.main.orthographicSize * Screen.width / Screen.height,2*Camera.main.orthographicSize);
        //ammoCounter = 0;
        //EnterPrompt.rectTransform.anchoredPosition = 250 * Vector3.down;
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
    
    public Vector3 randomEmpty()
    {
        Vector3Int x;
        for (x = new Vector3Int(Random.Range(-16, 16), Random.Range(-16, 16), 0); WallMap.HasTile(x); x = new Vector3Int(Random.Range(-16, 16), Random.Range(-16, 16), 0));
        return Zone.GetCellCenterWorld(x);
        
    }
    public void spawnZombie()
    {
        cameraDimensions = new Vector2(2 * Camera.main.orthographicSize * Screen.width / Screen.height, 2 * Camera.main.orthographicSize);
        if (pool.Count > 0)
        {
            //pool.Peek().transform.localScale = new Vector3(1, 1, 1);
            //pool.Peek().transform.SetParent(transform);
            //pool.Peek().transform.position = spawners.transform.GetChild(Random.Range(0, spawners.transform.childCount)).transform.position + Random.Range(-.25f, .25f) * new Vector3(1, 1, 0);
            pool.Peek().transform.position = squareAroundPlayer(cameraDimensions + new Vector2(2, 2));
            //pool.Peek().GetComponent<CircleCollider2D>().enabled = true;
            //pool.Pop().GetComponent<pathfinding>().enabled = true;
            //pool.Peek().
            pool.Pop().SetActive(true);

        }
        else
        {
            Instantiate(zombie, squareAroundPlayer(cameraDimensions + new Vector2(2, 2)), Quaternion.Euler(0, 0, 0), transform);
        }
    }
    public IEnumerator SpawnZombiesIndefinitely(float interval)
    {
        
        upgradeCounter = 0;
        while (upgradeCounter<wave*2+1)
        {
            if (transform.childCount - pool.Count <= wave+2)
            {
                spawnZombie();
            }
            yield return new WaitForSeconds(interval);
        }
        yield break;
    }
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
            //pool.Peek().transform.localScale = new Vector3(1, 1, 1);
            //pool.Peek().transform.SetParent(transform);
            //pool.Peek().transform.position = spawners.transform.GetChild(Random.Range(0, spawners.transform.childCount)).transform.position + Random.Range(-.25f, .25f) * new Vector3(1, 1, 0);
            pool.Peek().transform.position = squareAroundPlayer(cameraDimensions + new Vector2(2, 2));
            //pool.Peek().GetComponent<CircleCollider2D>().enabled = true;
            //pool.Pop().GetComponent<pathfinding>().enabled = true;
            pool.Pop().SetActive(true);
            yield return new WaitForSeconds(interval);
            //pool.Pop().SetActive(true);

        }
        yield break;
    }
    public void incrementWave()
    {
        if (pool.Count == transform.childCount&&!upgrader.activeSelf)
        {
            //WaveIndicator.text = "wave: " + wave;
            upgradeCounter = 0;
            upgrader.transform.GetChild(0).localScale = new Vector3(0, 0, 0);
            upgrader.SetActive(true);
            upgrader.transform.position = randomEmpty();
            //upgradeCounter = 0;
            //EnterPrompt.rectTransform.anchoredPosition = new Vector3(0,-300,0);
            EnterPrompt.enabled = false;
            upgradeMessage.enabled = false;
            //Debug.Log("new wave");
            wave += 1;
            points = 0;
            //pointLimit = wave * 5;
            //PointsBar.anchorMax = new Vector2(points / pointLimit, 1);

            IndefiniteSpawner = SpawnZombiesIndefinitely( 1+4/wave);
            WaveIndicator.text = "wave: " + wave;
            //StartCoroutine(SpawnZombies(wave, 0.5f));
            //StartCoroutine(SpawnZombiesIndefinitely(5/wave));
            StartCoroutine(IndefiniteSpawner);
            //spawnzombies(wave);
        }
    }

    void randomUpgrade()
    {
        //Debug.Log("Damage =="+ (25f + wave * 10f) / wave);
        switch (Random.Range(0,9))
        {
            case 0:
                if (gs.equipped.FireRate >= 50)
                {
                    randomUpgrade();
                    break;
                }
                gs.equipped.FireRate = Mathf.Min(50, gs.equipped.FireRate * (1 + points / 20.0f));
                upgradeMessage.text = "firing rate increased! x"+(1 + points / 20.0f).ToString();
                break;

            case 1:
                if (gs.equipped.Spread < 5)
                {
                    randomUpgrade();
                    break;
                }
                
                gs.equipped.Spread /= 1.5f;
                //upgradeMessage.text = "accuracy improved! x" + 1.5f;
                upgradeMessage.text = "accuracy x" + 1.5f;
                break;

            case 2:
                if (gs.equipped.Speed >= 50)
                {
                    randomUpgrade();
                    break;
                }
                gs.equipped.Speed = Mathf.Min(50, gs.equipped.Speed * (1 + points / 20.0f));
                //upgradeMessage.text = "Bullet velocity increased! x" + (1 + points / 10).ToString();
                upgradeMessage.text = "Bullet velocity x" + (1 + points / 20.0f).ToString();
                break;

            case 3:
                gs.equipped.Recoil /= 1.5f;
                upgradeMessage.text = "Recoil -1/3";
                break;

            case 4:
                if (gs.equipped.Bullets > 5)
                {
                    randomUpgrade();
                    break;
                }
                gs.equipped.Bullets += Mathf.CeilToInt(points/20.0f);
                upgradeMessage.text = "Multi-shot! +" + (Mathf.CeilToInt(points/20.0f)).ToString();
                break;

            case 5:
                gs.equipped.Damage += (float)points/wave;// points;
                //Debug.Log("Damage +" + 10.0f / (5 * points * (5 * wave + 2)));
                upgradeMessage.text = "Damage +" + (float)points/wave;// 10.0f/(5*points*(5*wave+2));
                break;

            case 6:
                gs.equipped.ClipSize += points;
                upgradeMessage.text = "Magazine size +" + points;
                break;

            case 7:
                if (gs.equipped.Knockback >= 50)
                {
                    randomUpgrade();
                    break;
                }
                gs.equipped.Knockback = Mathf.Min(50, gs.equipped.Knockback+points);
                upgradeMessage.text = "Knockback +" + points;
                break;

            case 8:
                gs.equipped.ReloadTime /= 1.5f;
                upgradeMessage.text = "reload time -1/3";
                break;


        }
        //points = 0;
        //PointsUI.text = "points: " + points;
    }

    public void EndWave()
    {
        EnterPrompt.enabled = true;
        //upgradeMessage.enabled = true;
        //upgrader.transform.GetChild(0).localScale = new Vector3(0, 0, 0);
        upgrader.SetActive(false);
        StopAllCoroutines();
        randomUpgrade();
        /*switch (0)
        {
            case 0:
                
                gs.equipped.FireRate = Mathf.Min(50,gs.equipped.FireRate*(1 + ps.points/100));
                Debug.Log("firerate");
                upgradeMessage.text = "firing rate increased!";
                break;
            case 1:
                gs.equipped.Spread /= 1.5f;
                Debug.Log("spread");
                upgradeMessage.text = "accuracy improved!";
                break;
            case 2:
                gs.equipped.Speed += 5;
                Debug.Log("speed");
                upgradeMessage.text = "Bullet velocity increased!";
                break;
            case 3:
                gs.equipped.Recoil /= 2;
                Debug.Log("recoil");
                upgradeMessage.text = "Recoil reduced!";
                break;
            case 4:
                gs.equipped.Bullets += 1;
                Debug.Log("bullets");
                upgradeMessage.text = "Multi-shot!";
                break;
            case 5:
                gs.equipped.Damage += 15;
                Debug.Log("dam");
                upgradeMessage.text = "Bullet damage increased!";
                break;
            case 6:
                gs.equipped.ClipSize += 8;
                Debug.Log("clip");
                upgradeMessage.text = "Magazine size increased!";
                break;
            case 7:
                gs.equipped.Knockback += 7;
                Debug.Log("nok");
                upgradeMessage.text = "bullet knockback increased!";
                break;
            case 8:
                gs.equipped.ReloadTime /= 1.5f;
                Debug.Log("time");
                upgradeMessage.text = "reload shortened!";
                break;


        }*/
        //ps.points = 0;
        //ps.PointsUI.text = "points: " + ps.points;
        /*switch (1)
        {
            case 0:
                gs.equipped.FireRate *= 1.25f;
                Debug.Log("firerate");
                upgradeMessage.text = "firing rate increased!";
                break;
            case 1:
                gs.equipped.Spread /= 1.5f;
                Debug.Log("spread");
                upgradeMessage.text = "accuracy improved!";
                break;
            case 2:
                gs.equipped.Speed += 5;
                Debug.Log("speed");
                upgradeMessage.text = "Bullet velocity increased!";
                break;
            case 3:
                gs.equipped.Recoil /= 2;
                Debug.Log("recoil");
                upgradeMessage.text = "Recoil reduced!";
                break;
            case 4:
                gs.equipped.Bullets += 1;
                Debug.Log("bullets");
                upgradeMessage.text = "Multi-shot!";
                break;
            case 5:
                gs.equipped.Damage += 15;
                Debug.Log("dam");
                upgradeMessage.text = "Bullet damage increased!";
                break;
            case 6:
                gs.equipped.ClipSize += 8;
                Debug.Log("clip");
                upgradeMessage.text = "Magazine size increased!";
                break;
            case 7:
                gs.equipped.Knockback += 7;
                Debug.Log("nok");
                upgradeMessage.text = "bullet knockback increased!";
                break;
            case 8:
                gs.equipped.ReloadTime /= 1.5f;
                Debug.Log("time");
                upgradeMessage.text = "reload shortened!";
                break;


        }*/
        upgradeMessage.enabled = true;
        //gs.equipped.Bullets +=1;
        gs.equipped.Spread = (gs.equipped.Bullets-1)*15;
    }
    // Update is called once per frame
    void Update()
    {
        //GameObject.Find("GameObject").transform.position = (Vector2)Camera.main.transform.position;
        //GameObject.Find("GameObject").transform.localScale = new Vector2(1, 1);// cameraDimensions-new Vector2(2,2);
        if (upgrader.activeSelf && upgradeCounter >= wave*2+1)
        {
            //EnterPrompt.enabled = true;
            /*if (pool.Count < transform.childCount)
            {
                EnterPrompt.text = "fight off/evade the remaining zombies";
            }*/
            //else
            //{
            if (pool.Count == transform.childCount)
            {
                EndWave();
            }
            //}

        }
        else if(Vector3.Distance(ps.transform.position, upgrader.transform.position) <= 2.5f)
        {
            upgradeCounter += Time.deltaTime;
            upgrader.transform.GetChild(0).localScale = new Vector3(upgradeCounter / (wave * 2 + 1), upgradeCounter / (wave * 2 + 1), 0);

        }
        /*if (upgrader.activeSelf&&Vector3.Distance(ps.transform.position, upgrader.transform.position)<=2.5f)
        {
            upgradeCounter += Time.deltaTime;

            if (upgradeCounter >= 3)
            {
                EndWave();
                
            }
        }*/
        /*if (upgradeCounter >= 3)
        {
            EndWave();
            //StopCoroutine("SpawnZombiesIndefinitely");
            //EnterPrompt.enabled = true;
            //upgrader.SetActive(false);
            //StopAllCoroutines();
            //upgradeCounter = 0;
            //gs.equipped.Bullets += 1;
        }*/
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
        //WaveIndicator.text = "wave: " + wave;
        /*if (pool.Count == transform.childCount)
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
        //}
        /*else {
            //EnterPrompt.rectTransform.anchoredPosition = new Vector3(0, EnterPrompt.rectTransform.anchoredPosition.y+(-250 - EnterPrompt.rectTransform.anchoredPosition.y) /3, 0);
            /*if (Mathf.FloorToInt(EnterPrompt.rectTransform.anchoredPosition.y) != -350)
            {*/
            //EnterPrompt.rectTransform.position += new Vector3(0, (-350 - EnterPrompt.rectTransform.anchoredPosition.y) / 3, 0);
            //}
        //}
    }
}
