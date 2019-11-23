using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombiescript : MonoBehaviour
{
    public float health;
    //public Vector2 velocity;
    // Start is called before the first frame update
    void Start()
    {
        health = 100.0f;
        //velocity = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            //Destroy(GetComponent<pathfinding>().HighlightMap);
            //Destroy(gameObject);
            GameObject.Find("Zombies").GetComponent<waves>().pool.Push(gameObject);
            health = 100;
            //gameObject.GetComponent<pathfinding>().enabled = false;
            gameObject.SetActive(false);
            transform.SetParent(GameObject.Find("ZombiePool").transform);

        }
    }
}
