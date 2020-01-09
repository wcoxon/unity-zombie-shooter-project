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
        ClipSize = clip;
        magazine = clip;
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
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Ammo")
        {
            equipped.Ammo += equipped.ClipSize;
            magsUI.text = equipped.Ammo.ToString();
            
            ps.ammoScript.Pool.Push(other.gameObject);
            other.gameObject.SetActive(false);
        }
        
    }

    void shoot(Gun gun)
    {
        
        
        if (gun.magazine == 0)
        {
            return;
        }
        
        if (nextFire <= Time.time && reloadTimer < Time.time)
        {
            
            nextFire = Time.time + 1 / gun.FireRate;
            
            ps.velocity -= ps.normalise(ps.aimvector) * gun.Recoil;
            
            gun.magazine-= 1;
            
            bulletsUI.text = equipped.magazine.ToString();
            for (int x = 0; x < gun.Bullets; x++)
            {
                
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
                

            }
        }
    }
    
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
            
            reloadTimer = Time.time + equipped.ReloadTime;
        }
        if (reloadTimer > Time.time)
        {
            
            reloadBarContainer.SetActive(true);
            
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
            
        }
        if (Input.GetMouseButtonDown(0) && nextFire <= Time.time)
        {
            
            nextFire = Time.time;
            
        }
        

    }
    
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            shoot(equipped);
        }
    }
}
