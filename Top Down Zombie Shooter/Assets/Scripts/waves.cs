using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waves : MonoBehaviour
{
    public int wave;

    //public GameObject pf;
    public pathfindingfunction pf;
    public UnityEngine.Tilemaps.Tilemap WallMap;

    public GameObject zombie;
    public GameObject spawners;
    public Stack<GameObject> pool;
    public UnityEngine.UI.Text WaveIndicator;
    // Start is called before the first frame update
    void Start()
    {
        
        pool = new Stack<GameObject>();
    }
    public void clearZombies()
    {
        foreach (Transform child in transform)///////////
        {
            Destroy(child.gameObject);
        }
    }
    public void spawnzombies(int number)
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
    }
    // Update is called once per frame
    void Update()
    {
        //GameObject.Find("wave").GetComponent<UnityEngine.UI.Text>().text = "wave: " + wave;
        WaveIndicator.text = "wave: " + wave;
        if (pool.Count == transform.childCount)
        {
            //Debug.Log("new wave");
            wave += 1;
            spawnzombies(wave);
        }
    }
}
