using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombiescript : MonoBehaviour
{
    public float health;
    public waves WaveScript;
    
    public CircleCollider2D coll;
    public Rigidbody2D rb;
    public Animator animator;
    
    private void Awake()
    {
        WaveScript = GameObject.Find("Zombies").GetComponent<waves>();
    }
    
    private void OnEnable()
    {
        
        health = 25+WaveScript.wave*10;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            
            animator.SetTrigger("Hit");
            
        }
    }
        // Update is called once per frame
        void FixedUpdate()
    {
        
        if(!new Rect((Vector2)Camera.main.transform.position- (WaveScript.cameraDimensions+new Vector2(4,4))/2, WaveScript.cameraDimensions + new Vector2(4, 4)).Contains(transform.position))
        {
            
            WaveScript.pool.Push(gameObject);
            gameObject.SetActive(false);
        }
        rb.velocity += rb.velocity * -0.25f;
        if (health <= 0)
        {
            animator.Rebind();
            WaveScript.points += 1;
            
            WaveScript.PointsUI.text = "Points: " + WaveScript.points;
            
            rb.velocity = new Vector2(0, 0);
            
            WaveScript.pool.Push(gameObject);
            
            
            gameObject.SetActive(false);
            

        }
    }
}
