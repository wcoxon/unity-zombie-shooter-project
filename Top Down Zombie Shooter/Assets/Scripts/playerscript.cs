using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class playerscript : MonoBehaviour
{
    public Vector2 velocity;
    public float acceleration = 40.0f;
    public float maxspeed = 10.0f;
    public Vector2 input;
    public Vector2 aimvector;
    public Transform Reticle;
    public float health;
    public waves waveScript;
    public Vector2 wallNormal;
    public Rigidbody2D rb;
    public UnityEngine.UI.Text HealthIndicator;
    public Pickups ammoScript;
    public Pickups healthScript;
    Vector3 cameraposition;
    public RectTransform Healthbar;
    public GameObject GameOver;
    public GameObject PauseMenu;
    float maxHealth;
    void Start()
    {
        //set maxHealth to 100
        maxHealth = 100;
        //set health to maxHealth
        health = maxHealth;
        
        
        UpdateHealthUI();
        //set cameraposition to 0,0,0
        cameraposition = new Vector3(0,0,0);
        //set wallNormal to 0,0
        wallNormal = new Vector2(0, 0);
        //make cursor invisible
        Cursor.visible = false;
        //set input to 0,0
        input = new Vector2(0, 0);
        //set velocity to 0,0 as the player should start motionless
        velocity = new Vector2(0, 0);
    }

    void UpdateHealthUI()
    {
        //set the health bar to fill the fraction of it's container equal to the player's health divided by the maximum health
        Healthbar.anchorMax = new Vector2(health / maxHealth, 1);
        //set the text within the health bar to show the correct health number value
        HealthIndicator.text = "health: " + health;
    }
    // Update is called once per frame
    //the float type normalise function will take the x and y components of a vector and return the x component of the vector were it to have magnitude 1
    float normalise(float x, float y)
    {
        //if the x component of the vector is 0, the normalised component is also 0, this must be returned beforehand, as if both x and y values are 0, the following operation would be dividing by zero
        if (x == 0)
        {
            return 0;
        }
        //return the x component divided by the magnitude of the vector given
        return x / Mathf.Pow(x * x + y * y, 0.5f);
    }
    //this overload of the normalise function takes a vector2, and returns a vector2. it also uses the float version of the normalise function
    public Vector2 normalise(Vector2 vector)
    {
        //returns the normalised x value and the normalised y value together in a vector2 construction
        return new Vector2(normalise(vector.x, vector.y), normalise(vector.y, vector.x));
    }
    
    //to avoid the player scraping slowly along walls using the unity physics, this gets the direction of the normal of the collision, this is added to the input so that input directed into a wall is disregarded,which makes walls essentially frictionless with the player
    void OnCollisionStay2D(Collision2D collision)
    {
        wallNormal = new Vector2(0, 0);
        for (int x = 0; x < collision.contacts.Length; x++)
        {
            wallNormal += new Vector2(collision.contacts[x].normal.x, collision.contacts[x].normal.y) / collision.contacts.Length;
        }
        wallNormal = normalise(wallNormal);
        

    }
    //the wallNormal is set back to 0,0 when no longer trying to crash into a wall
    private void OnCollisionExit2D(Collision2D collision)
    {
        wallNormal = new Vector2(0, 0);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
       
        //if youre touching a zombie's trigger, you are slowed down significantly, this gives zombies a sense of challenge, considering their slow attack speed, it also gives the impression that these zombies will grab onto you if you get too close
        if (other.tag == "Zombie")
        {
            maxspeed = 3f;
        }
        //if you touch a health pickup
        if (other.gameObject.tag == "Health")
        {
            //the pickup is pooled
            healthScript.Pool.Push(other.gameObject);
            //the player's health will increase by 10 but limit to maxHealth
            health = Mathf.Min(maxHealth, health + 10);
            //health UI is updated to show new health value
            UpdateHealthUI();
            //pickup is deactivated for later use
            other.gameObject.SetActive(false);

        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //when no longer touching a zombie your maximum speed returns to 9
        if (other.tag =="Zombie")
        {
            maxspeed = 9f;
        }
    }

    // Update is called once per frame
    
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if the pause menu is open, unpause, hide cursor and close pause menu
            if (PauseMenu.activeSelf)
            {
                Time.timeScale = 1.0f;
                Cursor.visible = false;
                
                PauseMenu.SetActive(false);
            }
            //if pause menu isnt open, freeze time, show cursor and open the pause menu
            else
            {
                Time.timeScale = 0.0f;
                Cursor.visible = true;
                
                PauseMenu.SetActive(true);
            }
        }
        //if paused, exit the update, this means that the player will not rotate when paused
        if (PauseMenu.activeSelf)
        {
            return;
        }
        //if pressed space, run the incrementWave function from waveScript, this doesnt necessarily increment the wave, but it will if the player is in between waves
        if (Input.GetKeyDown(KeyCode.Space))
        {
            waveScript.incrementWave();
        }

        //if player health drops to 0 or below
        if (health <= 0)
        {

            // deactivate the zombie's parent thus deactivating all zombies
            waveScript.gameObject.SetActive(false);
            //open the Game Over menu
            GameOver.SetActive(true);
            //show cursor
            Cursor.visible = true;
            //deactivate player
            gameObject.SetActive(false);
            
        }
        //set the input values to the horizontal and vertical inputs for player movement
        input.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //the wallNormal is added to the input and input is normalised, this means the input becomes the resultant direction of the input and the collided wall's normal direction
        input = normalise(normalise(input) + normalise(wallNormal));
        
        //if no input is given, the player will decelerate
        if (input.magnitude ==0)
        {
            
            velocity = normalise(velocity) * Mathf.Max(velocity.magnitude - acceleration * Time.deltaTime, 0);
        }
        //if input is given, player velocity will increase in the direction of the input, and by acceleration
        else
        {
            
            velocity += normalise(input) * acceleration * Time.deltaTime;
            
        }

        //velocity is set to the same direction but with a magnitude limited at maxspeed, so that the maxspeed is not exceeded
        velocity = normalise(velocity) * Mathf.Min(velocity.magnitude, maxspeed);

        //finally the player's rigidbody is moved to a position equal to its current position plus it's velocity *Time.deltaTime, Time.deltaTime ensures it increases by that amount every second, instead of every frame
        rb.MovePosition(transform.position+new Vector3(velocity.x, velocity.y)*Time.deltaTime);

        //camera position is set to the player's position on x and y axis
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10.0f);

        //aimvector is set to the mouse position - the player position, in other words it becomes a vector from the player to the mouse
        aimvector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        //using the aimvector, the player is rotated to the same direction as the aimvector, so the player points towards the mouse
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(aimvector.y, aimvector.x) * 180 / Mathf.PI - 90.0f);

        //cameraposition is moved towards the point halfway along the aimvector, the amount its moved by is proportional to the distance from cameraposition to half of aimvector, so the camera movement is smooth
        cameraposition = Vector3.MoveTowards((Vector2)cameraposition,normalise(aimvector) * aimvector.magnitude/2, Vector2.Distance(cameraposition, normalise(aimvector) * aimvector.magnitude / 2) *5*Time.deltaTime) + Vector3.forward * -10;
        //the camera's position is set to cameraposition
        Camera.main.transform.position = transform.position+cameraposition + Vector3.forward * -10;
        //the aiming reticle position is set to the mouse position in worldspace
        Reticle.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
    }
}
