using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class waves : MonoBehaviour
{
    public int wave;
    
    public UnityEngine.Tilemaps.Tilemap Ground;
    public UnityEngine.Tilemaps.Tilemap Highlighter;
    public UnityEngine.Tilemaps.Tile HighlighterTile;
    
    public pathfindingfunction pf;
    public UnityEngine.Tilemaps.Tilemap WallMap;
    public UnityEngine.Tilemaps.TilemapCollider2D WallCol;
    public playerscript ps;
    public guns gs;
    public GameObject zombie;
    
    public Stack<GameObject> pool;
    public UnityEngine.UI.Text WaveIndicator;
    public UnityEngine.UI.Text EnterPrompt;
    public UnityEngine.UI.Text upgradeMessage;
    public Vector2 cameraDimensions;
    public GameObject Airdrop;

    public int points;
    
    public UnityEngine.UI.Text PointsUI;
    

    public float CaptureCounter;
    IEnumerator IndefiniteSpawner;
    
    void Start()
    {
        cameraDimensions = new Vector2( 2 * Camera.main.orthographicSize * Screen.width / Screen.height,2*Camera.main.orthographicSize);
        
        pool = new Stack<GameObject>();
    }
    public void clearZombies()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    public Vector3Int squareAroundPlayer(Vector2 dimensions)
    {
        
        List<Vector3Int> cells = new List<Vector3Int>();
        
        Vector3Int BL = Ground.WorldToCell(Camera.main.transform.position - (Vector3)dimensions / 2);
        Vector3Int TR = Ground.WorldToCell(Camera.main.transform.position + (Vector3)dimensions / 2);

        for (int y = BL.y; y <= TR.y; y++)
        {
            for (int x = BL.x; x <= TR.x; x++)
            {
                if ((BL.x == x||BL.y==y||TR.x==x||TR.y==y)&&!WallMap.HasTile(Vector3Int.right * x + Vector3Int.up * y) && Ground.HasTile(Vector3Int.right * x + Vector3Int.up * y))
                {
                    
                    cells.Add(Vector3Int.up * y + Vector3Int.right * x);
                }
            }
        }
        return cells[Random.Range(0,cells.Count)];
        

    }
    
    
    public Vector3 randomEmpty()
    {
        Vector3Int x;
        for (x = new Vector3Int(Random.Range(-16, 16), Random.Range(-16, 16), 0); WallMap.HasTile(x); x = new Vector3Int(Random.Range(-16, 16), Random.Range(-16, 16), 0));
        return Ground.GetCellCenterWorld(x);
        
    }
    public void spawnZombie()
    {
        cameraDimensions = new Vector2(2 * Camera.main.orthographicSize * Screen.width / Screen.height, 2 * Camera.main.orthographicSize);
        if (pool.Count > 0)
        {
            
            pool.Peek().transform.position = squareAroundPlayer(cameraDimensions + new Vector2(2, 2));
            
            pool.Pop().SetActive(true);

        }
        else
        {
            Instantiate(zombie, squareAroundPlayer(cameraDimensions + new Vector2(2, 2)), Quaternion.Euler(0, 0, 0), transform);
        }
    }
    public IEnumerator SpawnZombiesIndefinitely(float interval)
    {
        
        CaptureCounter = 0;
        while (CaptureCounter<wave*2+1)
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
        
        int instantiate = Mathf.Max(number - pool.Count, 0);
        int reuse = Mathf.Min(pool.Count, number);

        
        for (int x = 0; x < instantiate; x++)
        {
            
            Instantiate(zombie, squareAroundPlayer(cameraDimensions+new Vector2(2,2)), Quaternion.Euler(0, 0, 0), transform);
            yield return new WaitForSeconds(interval);
        }
        for (int x = 0; x < reuse; x++)
        {
            
            pool.Peek().transform.position = squareAroundPlayer(cameraDimensions + new Vector2(2, 2));
            
            pool.Pop().SetActive(true);
            yield return new WaitForSeconds(interval);
            

        }
        yield break;
    }
    public void incrementWave()
    {
        if (pool.Count == transform.childCount&&!Airdrop.activeSelf)
        {

            CaptureCounter = 0;
            Airdrop.transform.GetChild(0).localScale = new Vector3(0, 0, 0);
            Airdrop.SetActive(true);
            Airdrop.transform.position = randomEmpty();

            EnterPrompt.enabled = false;
            upgradeMessage.enabled = false;
            
            wave += 1;
            points = 0;
            

            IndefiniteSpawner = SpawnZombiesIndefinitely( 1+4/wave);
            WaveIndicator.text = "wave: " + wave;
            
            StartCoroutine(IndefiniteSpawner);
            
        }
    }

    void randomUpgrade()
    {
        
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
                
                gs.equipped.Spread /= 1+points/20.0f;
                
                upgradeMessage.text = "accuracy x" + (1 + points / 20.0f);
                break;

            case 2:
                if (gs.equipped.Speed >= 50)
                {
                    randomUpgrade();
                    break;
                }
                gs.equipped.Speed = Mathf.Min(50, gs.equipped.Speed * (1 + points / 20.0f));
                
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
                gs.equipped.Damage += (float)points/wave;
                
                upgradeMessage.text = "Damage +" + (float)points/wave;
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
        
    }

    public void EndWave()
    {
        EnterPrompt.enabled = true;
        
        Airdrop.SetActive(false);
        StopAllCoroutines();
        randomUpgrade();
        
        upgradeMessage.enabled = true;
        
    }
    
    void Update()
    {
        
        if (Airdrop.activeSelf && CaptureCounter >= wave*2+1)
        {
            
            if (pool.Count == transform.childCount)
            {
                EndWave();
            }
            

        }
        else if(Vector3.Distance(ps.transform.position, Airdrop.transform.position) <= 2.5f)
        {
            CaptureCounter += Time.deltaTime;
            Airdrop.transform.GetChild(0).localScale = new Vector3(CaptureCounter / (wave * 2 + 1), CaptureCounter / (wave * 2 + 1), 0);

        }
        
    }
}
