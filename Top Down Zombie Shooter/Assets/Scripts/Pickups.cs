using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    public UnityEngine.Tilemaps.Tilemap Zone;
    public UnityEngine.Tilemaps.Tilemap WallMap;
    public float ammoCounter;
    public GameObject ammo;
    public Transform ammoParent;
    public int ammoLimit;
    public Stack<GameObject> ammoPool;
    public waves waveScript;
    // Start is called before the first frame update
    void Start()
    {
        ammoCounter = 0;
        ammoPool = new Stack<GameObject>();
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

        ammoCounter += Time.deltaTime;
        if (ammoCounter >= 1&&ammoParent.childCount-ammoPool.Count<ammoLimit&& waveScript.pool.Count < waveScript.transform.childCount)
        {
            if (ammoPool.Count > 0)
            {
                ammoPool.Peek().transform.position = randomEmpty();
                ammoPool.Pop().SetActive(true);
            }
            else
            {
                Instantiate(ammo, randomEmpty(), Quaternion.Euler(0, 0, 0), ammoParent);
            }
            ammoCounter = 0;
        }
    }
}
