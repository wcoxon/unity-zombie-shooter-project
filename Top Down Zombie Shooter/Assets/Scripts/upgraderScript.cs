using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upgraderScript : MonoBehaviour
{
   // public UnityEngine.Tilemaps.Tilemap Zone;
    //public UnityEngine.Tilemaps.Tilemap WallMap;
    // Start is called before the first frame update
    private void OnEnable()
    {
        //transform.position = randomEmpty();
    }
    /*public Vector3 randomEmpty()
    {
        Vector3Int x;
        for (x = new Vector3Int(Random.Range(-16, 16), Random.Range(-16, 16), 0); WallMap.HasTile(x); x = new Vector3Int(Random.Range(-16, 16), Random.Range(-16, 16), 0)) ;
        return Zone.GetCellCenterWorld(x);

    }*/
    // Update is called once per frame
    void Update()
    {
        
    }
}
