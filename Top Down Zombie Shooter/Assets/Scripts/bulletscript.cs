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
    public Gun gun;
    public guns gs;
    float knockback;
    public void set(float _speed,GameObject _parent,float _range,float _damage,float _knockback)
    {
        speed = _speed;
        parent = _parent;
        range = _range;
        damage = _damage;
        knockback = _knockback;
    }
    
    
    private void OnEnable()
    {
        parent = GameObject.Find("player");
        gs = parent.GetComponent<guns>();
        gun = gs.equipped;
        startPosition = transform.position;
        set(gun.Speed, parent, gun.Range, gun.Damage,gun.Knockback);

        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.gameObject.tag == "Zombie")
        {
            
            
            other.attachedRigidbody.velocity = new Vector2(Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180+90), Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180+90))*knockback;
            
            other.gameObject.GetComponent<zombiescript>().health -= damage;
        }
        if(other.gameObject.tag == "Wall"||other.gameObject.tag == "Zombie")
        {
            
            if (!gs.bulletPool.Contains(gameObject))
            {
                gs.bulletPool.Push(gameObject);
            }
            gameObject.SetActive(false);
            
        }
        
    }

    
    void FixedUpdate()
    {
        
        if (new Vector2(transform.position.x - startPosition.x, transform.position.y - startPosition.y).magnitude < range)
        {
            transform.Translate(new Vector2(0, speed * Time.fixedDeltaTime));
            
        }
        else
        {
            
            if (!gs.bulletPool.Contains(gameObject))
            {
                gs.bulletPool.Push(gameObject);
            }
            gameObject.SetActive(false);
            
        }
    }
}
