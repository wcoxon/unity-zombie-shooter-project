using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waves : MonoBehaviour
{
    public int wave;
    public GameObject zombie;
    public GameObject spawners;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void spawnzombies(int number)
    {
        for(int x = 0; x < number; x++)
        {
            Instantiate(zombie, spawners.transform.GetChild(Random.Range(0, spawners.transform.childCount)).transform.position+Random.Range(-1.0f,1.0f)*new Vector3(1,1,0),Quaternion.Euler(0,0,0),transform);

        }
    }
    // Update is called once per frame
    void Update()
    {
        GameObject.Find("wave").GetComponent<UnityEngine.UI.Text>().text = "wave: " + wave;
        if (transform.childCount == 0)
        {
            wave += 1;
            spawnzombies(wave * 3);
        }
    }
}
