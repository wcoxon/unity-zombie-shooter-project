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
    public GameObject pter;
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
    //public int points;
    //public int pointLimit;
    //public UnityEngine.UI.Text PointsUI;
    //public UnityEngine.UI.Button restart;
    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        Healthbar.anchorMax = new Vector2(health / 100, 1);
        HealthIndicator.text = "health: " + health;
        cameraposition = new Vector3(0,0,0);
        wallNormal = new Vector2(0, 0);
        Cursor.visible = false;
        input = new Vector2(0, 0);
        velocity = new Vector2(0, 0);
    }

    // Update is called once per frame
    float normalise(float x, float y)
    {
        if (x == 0)
        {
            return 0;
        }
        return x / Mathf.Pow(x * x + y * y, 0.5f);
    }
    public Vector2 normalise(Vector2 vector)
    {
        return new Vector2(normalise(vector.x, vector.y), normalise(vector.y, vector.x));
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("coll");
        /*if(collision.gameObject.tag == "Zombie")
        {
            health -= 5;
        }*/
        //Debug.Log("hit");
        
        wallNormal = new Vector2(0, 0);
        for (int x = 0; x < collision.contacts.Length; x++)
        {
            wallNormal += new Vector2(collision.contacts[x].normal.x, collision.contacts[x].normal.y) / collision.contacts.Length;
        }
        wallNormal = new Vector2(normalise(wallNormal).x* Mathf.Abs(velocity.x), normalise(wallNormal).y * Mathf.Abs(velocity.y));// new Vector2(normalise(wallNormal).x * Mathf.Abs(velocity.x), normalise(wallNormal).y * Mathf.Abs(velocity.y));
        velocity += wallNormal;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        wallNormal = new Vector2(0, 0);
        for (int x = 0; x < collision.contacts.Length; x++)
        {
            wallNormal += new Vector2(collision.contacts[x].normal.x, collision.contacts[x].normal.y) / collision.contacts.Length;
        }
        wallNormal = new Vector2(normalise(wallNormal).x * Mathf.Abs(velocity.x), normalise(wallNormal).y * Mathf.Abs(velocity.y));
        //Debug.Log("test");
        //pter.transform.position = new Vector2(0, 0);// new Vector2(transform.position.x, transform.position.y);
        /*resistance = new Vector2(0, 0);
        for (int x = 0; x < collision.contacts.Length; x++)
        {
            resistance += new Vector2(collision.contacts[x].normal.x, collision.contacts[x].normal.y)/ collision.contacts.Length;
        }*/
        //resistance = normalise(resistance)*velocity.magnitude;// new Vector2(normalise(resistance).x*Mathf.Abs(velocity.x), normalise(resistance).y * Mathf.Abs(velocity.y));
        //velocity += resistance;
        //pter.transform.position = transform.position + new Vector3(normalise(resistance).x, normalise(resistance).y);
        //velocity += new Vector2(normalise(pter.transform.position).x * Mathf.Abs(velocity.x) * Time.deltaTime, normalise(pter.transform.position).y * Mathf.Abs(velocity.y) * Time.deltaTime);
        //pter.transform.position = new Vector2(transform.position.x,transform.position.y) + normalise(collision.contacts[0].normal);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
       
        //Debug.Log("trig");
        if (other.tag == "Water"|| other.tag == "Zombie")
        {
            maxspeed = 3f;
        }
        if (other.gameObject.tag == "Health")
        {
            //equipped.Ammo += equipped.ClipSize;
            //magsUI.text = equipped.Ammo.ToString();
            //ammoCounter.text = "ammo: " + equipped.magazine + "/" + equipped.Ammo;
            healthScript.Pool.Push(other.gameObject);

            health = Mathf.Min(100, health + 10);
            Healthbar.anchorMax = new Vector2(health / 100, 1);
            HealthIndicator.text = health + " HP";

            other.gameObject.SetActive(false);

        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Water" || other.tag =="Zombie")
        {
            maxspeed = 9f;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        wallNormal = new Vector2(0, 0);
    }

    // Update is called once per frame
    /*public void MainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("SampleScene");
    }*/
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenu.activeSelf)
            {
                Time.timeScale = 1.0f;
                //Application.Quit();
                PauseMenu.SetActive(false);
            }
            else
            {
                Time.timeScale = 0.0f;
                //Application.Quit();
                PauseMenu.SetActive(true);
            }
        }
        if (Time.timeScale == 0.0f)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            waveScript.incrementWave();
        }
        //GameObject.Find("health").GetComponent<UnityEngine.UI.Text>().text = "health: " + health;///////
        //HealthIndicator.text = "health: " + health;
        if (health <= 0)
        {

            //health = 100;
            //transform.position = new Vector3(0, 0, 0);
            //GameObject.Find("Zombies").GetComponent<waves>().wave = 0;//////////
            //waveScript.wave = 0;
            //waveScript.clearZombies();
            waveScript.gameObject.SetActive(false);
            GameOver.SetActive(true);
            Cursor.visible = true;
            gameObject.SetActive(false);
            //restart.onClick.AddListener( restartfunc());
            //SceneManager.LoadScene("SampleScene");
            //Application.LoadLevel(Application.loadedLevel);
            /*foreach (Transform child in GameObject.Find("Zombies").transform)///////////
            {
                Destroy(child.gameObject);
            }*/
        }
        //pter.transform.position = transform.position;

        //Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10.0f);
        //aimvector = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y);
        //input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input = normalise(normalise(input) + normalise(wallNormal));
        //input.x = Input.GetAxisRaw("Horizontal");
        //input.y = Input.GetAxisRaw("Vertical");
        //velocity += new Vector2(normalise(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * acceleration * Time.deltaTime, normalise(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal")) * acceleration * Time.deltaTime);

        if (input.magnitude ==0)
        {
            
            velocity = normalise(velocity) * Mathf.Max(velocity.magnitude - acceleration * Time.deltaTime, 0);
        }
        else
        {
            velocity += normalise(input) * acceleration * Time.deltaTime;///////////
            //velocity.x += normalise(input).x * acceleration * Time.deltaTime;
        }
        /*if (input.y == 0)
        {
            resistance.y = //-Mathf.Sign(velocity.y) * Mathf.Min(Mathf.Abs(normalise(velocity).y)*acceleration * Time.deltaTime, Mathf.Abs(velocity.y));
            //velocity = normalise(velocity) * Mathf.Max(velocity.magnitude - acceleration * Time.deltaTime, 0);
        }
        else
        {
            resistance.y = 0;
            //velocity.y += normalise(input).y * acceleration * Time.deltaTime;
        }*/
        
        //velocity += normalise(input) * acceleration * Time.deltaTime;
        /*else if(input.magnitude!=0)
        {
            velocity += normalise(input) * acceleration * Time.deltaTime;
            //velocity = normalise(velocity) * Mathf.Min(velocity.magnitude, maxspeed);
        }*/
        //velocity += resistance;
        //velocity = normalise(velocity) * velocity.magnitude;
        //velocity += resistance;
        velocity = normalise(velocity) * Mathf.Min(velocity.magnitude, maxspeed);////////////////

        //Camera.main.transform.position = new Vector3(transform.position.x/2 + Camera.main.ScreenToWorldPoint(Input.mousePosition).x/2, transform.position.y/2 + Camera.main.ScreenToWorldPoint(Input.mousePosition).y/2, -10.0f);
        //pter.transform.position = new Vector3( Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y,0);//(transform.position + Camera.main.ScreenToWorldPoint(Input.mousePosition))/2;

        //transform.eulerAngles = new Vector3(0,0,Mathf.Atan2(aimvector.y, aimvector.x) * 180 / Mathf.PI - 90.0f);
        rb.MovePosition(transform.position+new Vector3(velocity.x, velocity.y)*Time.deltaTime);/////////////


        //pter.transform.position = transform.position+new Vector3(wallNormal.x,wallNormal.y);
        //pter.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);

        //cameraposition = Camera.main.transform.position;
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10.0f);

        aimvector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;// new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y);


        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(aimvector.y, aimvector.x) * 180 / Mathf.PI - 90.0f);

        cameraposition = Vector3.MoveTowards((Vector2)cameraposition,normalise(aimvector) * aimvector.magnitude/2, Vector2.Distance(cameraposition, normalise(aimvector) * aimvector.magnitude / 2) *5*Time.deltaTime) + Vector3.forward * -10;// Vector2.Distance(Camera.main.transform.position,aimvector)) + Vector3.forward * -10;//new Vector3(transform.position.x + aimvector.x, transform.position.y + aimvector.y, -10);
        Camera.main.transform.position = transform.position+cameraposition + Vector3.forward * -10;

        pter.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);

        /*
        aimvector = Vector2.MoveTowards(aimvector,new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y),Time.deltaTime*20f*Vector3.Distance(aimvector, new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y)));
        
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(aimvector.y, aimvector.x) * 180 / Mathf.PI - 90.0f);
        Camera.main.transform.position = new Vector3(transform.position.x+aimvector.x, transform.position.y+aimvector.y, -10);//new Vector3(transform.position.x / 2 + Camera.main.ScreenToWorldPoint(Input.mousePosition).x / 2, transform.position.y / 2 + Camera.main.ScreenToWorldPoint(Input.mousePosition).y / 2, -10.0f);
        pter.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);*/
        //Debug.Log(pter.transform.position == transform.position);
    }
}
