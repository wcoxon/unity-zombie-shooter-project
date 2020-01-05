using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    public UnityEngine.Tilemaps.Tilemap Zone;
    public UnityEngine.Tilemaps.Tilemap WallMap;
    public float Counter;
    public GameObject pickup;
    public Transform Parent;
    public int Limit;
    public Stack<GameObject> Pool;
    public waves waveScript;
    //public float healthCounter;
    //public GameObject health;
    //public Transform healthParent;
    //public int healthLimit;
   // public Stack<GameObject> healthPool;
    // Start is called before the first frame update
    void Start()
    {
       // healthCounter = 0;
       // healthPool = new Stack<GameObject>();
        Counter = 0;
        Pool = new Stack<GameObject>();
    }
    public Vector3 randomEmpty()
    {
        Vector3Int x;
        for (x = new Vector3Int(Random.Range(-16, 16), Random.Range(-16, 16), 0); WallMap.HasTile(x); x = new Vector3Int(Random.Range(-16, 16), Random.Range(-16, 16), 0)) ;
        return Zone.GetCellCenterWorld(x);

    }
    // Update is called once per frame
    void Update()
    {

        Counter += Time.deltaTime;
        if (Counter >= 1&&Parent.childCount-Pool.Count<Limit&& waveScript.pool.Count < waveScript.transform.childCount)
        {
            if (Pool.Count > 0)
            {
                Pool.Peek().transform.position = randomEmpty();
                Pool.Pop().SetActive(true);
            }
            else
            {
                Instantiate(pickup, randomEmpty(), Quaternion.Euler(0, 0, 0), Parent);
            }
            Counter = 0;
        }
        /*
        healthCounter += Time.deltaTime;
        if (healthCounter >= 1 && healthParent.childCount - healthPool.Count < healthLimit && waveScript.pool.Count < waveScript.transform.childCount)
        {
            if (healthPool.Count > 0)
            {
                healthPool.Peek().transform.position = randomEmpty();
                healthPool.Pop().SetActive(true);
            }
            else
            {
                Instantiate(health, randomEmpty(), Quaternion.Euler(0, 0, 0), healthParent);
            }
            healthCounter = 0;
        }*/
    }
}
