using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            GameObject.Find("player").GetComponent<playerscript>().maxspeed = 5;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            GameObject.Find("player").GetComponent<playerscript>().maxspeed = 9;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
