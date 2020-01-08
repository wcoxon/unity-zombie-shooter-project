using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun
{
    public float FireRate;
    public float Spread;
    public float Range;
    public float Speed;
    public float Recoil;
    public int Bullets;
    public float Damage;
    public int Ammo;
    public int ClipSize;
    public int magazine;
    public float Knockback;
    public float ReloadTime;
    public Gun(float fireRate,float spread,float range,float speed,float recoil,int bullets,float damage,int ammo,int clip,float knockback,float reload)
    {
        FireRate = fireRate;
        Spread = spread;
        Range = range;
        Speed = speed;
        Recoil = recoil;
        Bullets = bullets;
        Damage = damage;
        Ammo = ammo;
        magazine = clip;// ammo;
        ClipSize = clip;
        Knockback = knockback;
        ReloadTime = reload;

    }
}

public class guns : MonoBehaviour
{
    public Stack<GameObject> bulletPool;
    public GameObject bullet;
    Gun machinegun;
    public float nextFire;
    Gun pistol;
    Gun weakGun;
    public Gun equipped;
    public GameObject filled;
    //public UnityEngine.UI.Text ammoCounter;
    //public float[] gunstuff = { 0, 0, 0, 0, 0, 0, 0 };
    //public GameObject FirePoint;
    public Vector3 bulletoffset;
    public Transform Bullets;
    public playerscript ps;
    public bulletscript bs;
    public GameObject ammoUI;
    UnityEngine.UI.Text bulletsUI;
    UnityEngine.UI.Text magsUI;
    UnityEngine.UI.Text reloadUI;
    GameObject reloadBarContainer;
    RectTransform reloadBar;
    public float reloadTimer;
    Gun reallyWeakGun;
    
    //public Gun equipped;
    // Start is called before the first frame update
    void Start()
    {
        reloadTimer = 0;
        bulletsUI = ammoUI.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();
        magsUI = ammoUI.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>();
        reloadUI = ammoUI.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>();
        reloadBarContainer = ammoUI.transform.GetChild(3).gameObject;
        reloadBar = reloadBarContainer.transform.GetChild(0).GetComponent<RectTransform>();
        nextFire = 0.0f;
        reallyWeakGun = new Gun(7, 20, 20, 7, 10, 1, 5, 72, 24, 2, 2);
        weakGun = new Gun(10, 10, 20, 13, 5, 1, 5,72,24,2,2);
        machinegun = new Gun(15, 5, 30, 19,3,1,5,72,64,3,1);
        pistol = new Gun(100, 10, 100, 10, 100, 5, 10,2000,100,5,1);
        equipped = reallyWeakGun;
        bs = bullet.GetComponent<bulletscript>();
        bulletPool = new Stack<GameObject>();
        bulletsUI.text = equipped.magazine.ToString();
        magsUI.text = equipped.Ammo.ToString();
        //ammoCounter.text = "ammo: " + equipped.magazine + "/" + equipped.Ammo;
        //bs = new bulletscript();
        //bs.speed = 
        //pistol = new Gun(gunstuff[0], gunstuff[1], gunstuff[2], gunstuff[3], gunstuff[4], (int)gunstuff[5],gunstuff[6]);
        //equipped = pistol;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Ammo")
        {
            equipped.Ammo += equipped.ClipSize;
            magsUI.text = equipped.Ammo.ToString();
            //ammoCounter.text = "ammo: " + equipped.magazine + "/" + equipped.Ammo;
            ps.ammoScript.Pool.Push(other.gameObject);
            other.gameObject.SetActive(false);
        }
        /*if (other.gameObject.tag == "Health")
        {
            //equipped.Ammo += equipped.ClipSize;
            //magsUI.text = equipped.Ammo.ToString();
            //ammoCounter.text = "ammo: " + equipped.magazine + "/" + equipped.Ammo;
            ps.healthScript.Pool.Push(other.gameObject);
            
            ps.health = Mathf.Min(100, ps.health + 5);
            
            other.gameObject.SetActive(false);
        }*/
    }

    void shoot(Gun gun)
    {
        
        //GameObject _bullet;
        //bulletscript _bs;
        //_bs = new bulletscript(gun.Speed,gameObject,gun.Range,gun.Damage);
        //_bs.speed =
        //Debug.Log(nextFire);
        //Debug.Log(bulletPool.Count);
        if (gun.magazine == 0)
        {
            return;
        }
        //if (nextFire <= Time.time&&reloadTimer<Time.time)
        //while(nextFire <= Time.time && reloadTimer < Time.time)
        //Debug.Log((Time.time - nextFire) * gun.FireRate);
        //for(;nextFire<=Time.time; nextFire += (1 / gun.FireRate))
        if (nextFire <= Time.time && reloadTimer < Time.time)
        {
            //Debug.Log(Time.time + " :: " + nextFire);
            //Debug.Log(Time.time+1/gun.FireRate);
            //Debug.Log(Time.time);
            //Debug.Log((Time.time - nextFire)-1/equipped.FireRate);
            //Debug.Log((Time.time-nextFire)*equipped.FireRate);
            //Debug.Log("shoot an extra " + equipped.FireRate / (Time.time - nextFire));
            //Debug.Log("bang " + 1/equipped.FireRate + " " + Time.time);
            //Debug.Log(bulletPool.Count);
            //bs.set(gun.Speed, gameObject, gun.Range, gun.Damage);
            //_bs = new bulletscript(gun.Speed, gameObject, gun.Range, gun.Damage);
            nextFire = Time.time + 1 / gun.FireRate;//+=  (1 / gun.FireRate);
            //GetComponent<playerscript>().velocity -= GetComponent<playerscript>().normalise(GetComponent<playerscript>().aimvector) * gun.Recoil;
            ps.velocity -= ps.normalise(ps.aimvector) * gun.Recoil;
            /*if (bulletPool.Count >= gun.Bullets)
            {
                for (int x = 0; x < gun.Bullets; x++)
                {
                    bulletPool.Peek().transform.position = gameObject.transform.GetChild(0).transform.position;
                    bulletPool.Peek().transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + Random.Range(-gun.Spread / 2, gun.Spread / 2));
                    bulletPool.Peek().SetActive(true);
                }
            }*/
            gun.magazine-= 1;
            //ammoCounter.text = "ammo: " + equipped.magazine + "/" + equipped.Ammo;
            bulletsUI.text = equipped.magazine.ToString();
            for (int x = 0; x < gun.Bullets; x++)
            {
                //_bullet = Instantiate(bullet, gameObject.transform.GetChild(0).transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z + Random.Range(-gun.Spread / 2, gun.Spread / 2)), Bullets);
                if (bulletPool.Count >= 1)
                {
                    bulletPool.Peek().transform.position = gameObject.transform.GetChild(0).transform.position;
                    bulletPool.Peek().transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + Random.Range(-gun.Spread / 2, gun.Spread / 2));
                    bulletPool.Pop().SetActive(true);
                }
                else
                {
                    Instantiate(bullet, gameObject.transform.GetChild(0).transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z + Random.Range(-gun.Spread / 2, gun.Spread / 2)), Bullets);
                }
                //_bullet.GetComponent<bulletscript>() = _bs;
                //_bullet.GetComponent<bulletscript>().parent = gameObject;
                //_bullet.GetComponent<bulletscript>().range = gun.Range;
                //_bullet.GetComponent<bulletscript>().damage = gun.Damage;

            }
        }
    }
    /*IEnumerator reload(float time)
    {
        yield return new WaitForSeconds(time);

    }*/
    private void Update()
    {
        if (equipped.magazine < equipped.ClipSize &&equipped.Ammo>0&& (Input.GetKeyDown(KeyCode.R)|| Input.GetMouseButtonDown(0)&& equipped.magazine<=0))
        {
            if (equipped.ClipSize - equipped.magazine > equipped.Ammo)
            {
                equipped.magazine += equipped.Ammo;
                equipped.Ammo = 0;
            }
            else
            {
                equipped.Ammo -= equipped.ClipSize - equipped.magazine;
                equipped.magazine = equipped.ClipSize;
            }
            //equipped.magazine += Mathf.Min(equipped.ClipSize - equipped.magazine, equipped.Ammo);

            //ammoCounter.text = "ammo: " + equipped.magazine + "/" + equipped.Ammo;
            //bulletsUI.text = equipped.magazine.ToString();
            //magsUI.text = equipped.Ammo.ToString();
            reloadTimer = Time.time + equipped.ReloadTime;
        }
        if (reloadTimer > Time.time)
        {
            //filled.transform.localScale = new Vector3(0.9f*(1-(nextFire-Time.time)), 0.9f*(1-(nextFire - Time.time)), 1);
            //filled.transform.localScale = new Vector3(0.9f*(1-(nextFire - Time.time)/equipped.ReloadTime),0.9f*( 1- (nextFire - Time.time) / equipped.ReloadTime), 1);
            reloadBarContainer.SetActive(true);
            //bulletsUI.text = (equipped.magazine* (1 - (reloadTimer - Time.time) / equipped.ReloadTime)).ToString();//.ToString();
            reloadBar.anchorMax = new Vector2((1 - (reloadTimer - Time.time) / equipped.ReloadTime), 1);
            reloadUI.enabled = true;
        }
        else
        {
            reloadBarContainer.SetActive(false);
            reloadUI.enabled = false;
            if (equipped.magazine == 0)
            {
                bulletsUI.color = new Color(1, 0, 0);
            }
            else
            {
                bulletsUI.color = new Color(1, 1, 1);
            }
            if (equipped.Ammo == 0)
            {
                magsUI.color = new Color(1, 0, 0);
            }
            else
            {
                magsUI.color = new Color(1, 1, 1);
            }
            bulletsUI.text = equipped.magazine.ToString();
            magsUI.text = equipped.Ammo.ToString();
            //filled.transform.localScale = new Vector3(0.9f, 0.9f, 1);
            //filled.transform.localScale = new Vector3(0.9f,0.9f, 1);
        }
        if (Input.GetMouseButtonDown(0) && nextFire <= Time.time)
        {
            //Debug.Log("Big");
            nextFire = Time.time;
            //Instantiate(bullet, transform.position,Quaternion.Euler(transform.rotation.x,transform.rotation.y, transform.rotation.z + Random.Range(-pistol.Spread/2,pistol.Spread/2)));
        }
        /*if (Input.GetMouseButton(0))
        {
            shoot(equipped);
            //Instantiate(bullet, transform.position,Quaternion.Euler(transform.rotation.x,transform.rotation.y, transform.rotation.z + Random.Range(-pistol.Spread/2,pistol.Spread/2)));
        }*/

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Time.timeScale = 0.5f;


        /*if (equipped.magazine < equipped.ClipSize&&Input.GetKeyDown(KeyCode.R))
        {
            if(equipped.ClipSize - equipped.magazine > equipped.Ammo)
            {
                equipped.magazine += equipped.Ammo;
                equipped.Ammo = 0;
            }
            else
            {
                equipped.Ammo -= equipped.ClipSize - equipped.magazine;
                equipped.magazine = equipped.ClipSize;
            }
            //equipped.magazine += Mathf.Min(equipped.ClipSize - equipped.magazine, equipped.Ammo);

            //ammoCounter.text = "ammo: " + equipped.magazine + "/" + equipped.Ammo;
            //bulletsUI.text = equipped.magazine.ToString();
            //magsUI.text = equipped.Ammo.ToString();
            reloadTimer = Time.time + equipped.ReloadTime;
        }
        if (reloadTimer > Time.time)
        {
            //filled.transform.localScale = new Vector3(0.9f*(1-(nextFire-Time.time)), 0.9f*(1-(nextFire - Time.time)), 1);
            //filled.transform.localScale = new Vector3(0.9f*(1-(nextFire - Time.time)/equipped.ReloadTime),0.9f*( 1- (nextFire - Time.time) / equipped.ReloadTime), 1);
            reloadBarContainer.SetActive(true);
            //bulletsUI.text = (equipped.magazine* (1 - (reloadTimer - Time.time) / equipped.ReloadTime)).ToString();//.ToString();
            reloadBar.anchorMax = new Vector2((1 - (reloadTimer - Time.time) / equipped.ReloadTime), 1);
            reloadUI.enabled = true;
        }
        else
        {
            reloadBarContainer.SetActive(false);
            reloadUI.enabled = false;
            bulletsUI.text = equipped.magazine.ToString();
            magsUI.text = equipped.Ammo.ToString();
            //filled.transform.localScale = new Vector3(0.9f, 0.9f, 1);
            //filled.transform.localScale = new Vector3(0.9f,0.9f, 1);
        }*/
        /*if (Input.GetMouseButtonDown(0)&&nextFire<=Time.time)
        {
            Debug.Log("Big");
            //nextFire = Time.time;
            //Instantiate(bullet, transform.position,Quaternion.Euler(transform.rotation.x,transform.rotation.y, transform.rotation.z + Random.Range(-pistol.Spread/2,pistol.Spread/2)));
        }*/
        if (Input.GetMouseButton(0))
        {
            shoot(equipped);
            //Instantiate(bullet, transform.position,Quaternion.Euler(transform.rotation.x,transform.rotation.y, transform.rotation.z + Random.Range(-pistol.Spread/2,pistol.Spread/2)));
        }
    }
}
