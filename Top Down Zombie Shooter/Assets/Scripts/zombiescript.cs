﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombiescript : MonoBehaviour
{
    public float health;
    public waves WaveScript;
    pathfinding pfScript;
    public CircleCollider2D coll;
    public Rigidbody2D rb;
    public Animator animator;
    //public GameObject pf;
    //public GameObject WallMap;
    //public GameObject Target;
    //public Vector2 velocity;
    // Start is called before the first frame update
    void Start()
    {

        pfScript = GetComponent<pathfinding>();
        //coll = GetComponent<CircleCollider2D>();
        //pf = GameObject.Find("pathfinder");
        WaveScript = GameObject.Find("Zombies").GetComponent<waves>();
        health = 100;
        GetComponent<pathfinding>().enabled = true;
        //velocity = new Vector2(0, 0);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            //health -= 5;
            //Debug.Log("Test");
            animator.SetTrigger("Hit");
            //animator.SetBool("Hit", true);
        }
    }
        // Update is called once per frame
        void FixedUpdate()
    {
        
        rb.velocity += rb.velocity * -0.25f;
        if (health <= 0)
        {
            //Destroy(GetComponent<pathfinding>().HighlightMap);
            //Destroy(gameObject);
            //GameObject.Find("Zombies").GetComponent<waves>().pool.Push(gameObject);
            WaveScript.pool.Push(gameObject);
            health = 100;
            //gameObject.GetComponent<pathfinding>().enabled = false;
            //gameObject.SetActive(false);
            //transform.localScale = new Vector3(0.5f,1,1);
            
            //GetComponent<pathfinding>().enabled = false;
            pfScript.enabled = false;
            coll.enabled = false;
            
            //GetComponent<CircleCollider2D>().enabled = false;
            //transform.SetParent(GameObject.Find("ZombiePool").transform);

        }
    }
}
