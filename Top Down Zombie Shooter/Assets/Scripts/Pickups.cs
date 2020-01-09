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
    public float interval;
    
    void Start()
    {
       
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
        if (Parent.childCount - Pool.Count < Limit)
        {
            Counter += Time.fixedDeltaTime;

            if (Counter >= interval && waveScript.pool.Count < waveScript.transform.childCount)
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
        }
        
    }
}
