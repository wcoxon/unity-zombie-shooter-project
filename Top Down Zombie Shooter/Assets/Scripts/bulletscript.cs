using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletscript : MonoBehaviour
{
    public float speed = 0.0f;
    public GameObject parent;
    public Vector3 startPosition;
    public float range = 0.0f;
    public float damage = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;

        //speed = 0.1f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("collided");
        if(other.gameObject.tag == "Zombie")
        {
            Debug.Log("collided");
            other.gameObject.GetComponent<zombiescript>().health -= damage;
        }
        if (other.gameObject != parent && !other.isTrigger)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (new Vector2(transform.position.x - startPosition.x, transform.position.y - startPosition.y).magnitude < range)
        {
            transform.Translate(new Vector2(0, speed * Time.deltaTime));
            //transform.Translate(new Vector2(0.5f*Mathf.Sin(2*Vector3.Distance(transform.position, startPosition)), speed * Time.deltaTime)); ;
        }
        else
        {
            //Destroy(GetComponent<pathfinding>().HighlightMap);
            Destroy(gameObject);
        }
    }
}
