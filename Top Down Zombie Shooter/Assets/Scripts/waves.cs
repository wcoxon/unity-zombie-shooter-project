using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waves : MonoBehaviour
{
    public int wave;

    //public GameObject pf;
    public pathfindingfunction pf;
    public UnityEngine.Tilemaps.Tilemap WallMap;
    public UnityEngine.Tilemaps.TilemapCollider2D WallCol;
    public playerscript ps;
    public GameObject zombie;
    public GameObject spawners;
    public Stack<GameObject> pool;
    public UnityEngine.UI.Text WaveIndicator;
    public UnityEngine.UI.Text EnterPrompt;
    // Start is called before the first frame update
    void Start()
    {
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
    public IEnumerator Spawnz(int number,float interval)
    {
        int instantiate = Mathf.Max(number - pool.Count, 0);
        int reuse = Mathf.Min(pool.Count, number);
        //Debug.Log("zombies " + number);
        //Debug.Log("instantiate " + (number - pool.Count) + " reuse " + pool.Count);
        //Debug.Log("reuse " + pool.Count);
        for (int x = 0; x < instantiate; x++)
        {
            Instantiate(zombie, spawners.transform.GetChild(Random.Range(0, spawners.transform.childCount)).transform.position + Random.Range(-.25f, .25f) * new Vector3(1, 1, 0), Quaternion.Euler(0, 0, 0), transform);
            yield return new WaitForSeconds(interval);
        }
        for (int x = 0; x < reuse; x++)
        {
            pool.Peek().transform.localScale = new Vector3(1, 1, 1);
            pool.Peek().transform.SetParent(transform);
            pool.Peek().transform.position = spawners.transform.GetChild(Random.Range(0, spawners.transform.childCount)).transform.position + Random.Range(-.25f, .25f) * new Vector3(1, 1, 0);
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
            StartCoroutine(Spawnz(wave, 0.5f));
            //spawnzombies(wave);
        }
    }
    // Update is called once per frame
    void Update()
    {
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
