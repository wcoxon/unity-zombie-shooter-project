using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombiescript : MonoBehaviour
{
    public float health;
    public waves WaveScript;
    //pathfinding pfScript;
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

        //pfScript = GetComponent<pathfinding>();
        //coll = GetComponent<CircleCollider2D>();
        //pf = GameObject.Find("pathfinder");
        WaveScript = GameObject.Find("Zombies").GetComponent<waves>();
        health = 100;
        GetComponent<pathfinding>().enabled = true;
        //velocity = new Vector2(0, 0);
    }
    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().color = new Color(51, 91, 59);
        health = 100;
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
        
        if(!new Rect((Vector2)Camera.main.transform.position- (WaveScript.cameraDimensions+new Vector2(4,4))/2, WaveScript.cameraDimensions + new Vector2(4, 4)).Contains(transform.position))
        {
            //cameraDimensions = new Vector2(2 * Camera.main.orthographicSize * Screen.width / Screen.height, 2 * Camera.main.orthographicSize);
            //Debug.Log(WaveScript.cameraDimensions);
            //pfScript.enabled = false;
            //GetComponent<SpriteRenderer>().color = new Color(0,0,1);
            //Debug.Log(new Rect((Vector2)Camera.main.transform.position, WaveScript.cameraDimensions).)
            /*pfScript.enabled = false;
            transform.position = WaveScript.squareAroundPlayer(WaveScript.cameraDimensions + new Vector2(2, 2));
            pfScript.enabled = true;*/
            WaveScript.pool.Push(gameObject);
            gameObject.SetActive(false);
        }
        rb.velocity += rb.velocity * -0.25f;
        if (health <= 0)
        {
            rb.velocity = new Vector2(0, 0);
            //Destroy(GetComponent<pathfinding>().HighlightMap);
            //Destroy(gameObject);
            //GameObject.Find("Zombies").GetComponent<waves>().pool.Push(gameObject);
            WaveScript.pool.Push(gameObject);
            health = 100;
            //gameObject.GetComponent<pathfinding>().enabled = false;
            //gameObject.SetActive(false);
            //transform.localScale = new Vector3(0.5f,1,1);
            //GetComponent<pathfinding>().enabled = false;
            
            gameObject.SetActive(false);
            //pfScript.enabled = false;
            //coll.enabled = false;
            //enabled = false;
            //GetComponent<CircleCollider2D>().enabled = false;
            //transform.SetParent(GameObject.Find("ZombiePool").transform);

        }
    }
}
