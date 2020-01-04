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
    public Gun(float fireRate,float spread,float range,float speed,float recoil,int bullets,float damage,int clip,float knockback)
    {
        FireRate = fireRate;
        Spread = spread;
        Range = range;
        Speed = speed;
        Recoil = recoil;
        Bullets = bullets;
        Damage = damage;
        Ammo = 72;
        magazine = clip;// ammo;
        ClipSize = clip;
        Knockback = knockback;

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
    public UnityEngine.UI.Text ammoCounter;
    //public float[] gunstuff = { 0, 0, 0, 0, 0, 0, 0 };
    //public GameObject FirePoint;
    public Vector3 bulletoffset;
    public Transform Bullets;
    public playerscript ps;
    public bulletscript bs;
    //public Gun equipped;
    // Start is called before the first frame update
    void Start()
    {

        nextFire = 0.0f;
        weakGun = new Gun(10, 10, 20, 13, 5, 1, 5,24,2);
        machinegun = new Gun(15, 5, 30, 19,3,1,5,64,3);
        pistol = new Gun(100, 100, 100, 10, 100, 5, 10,128,5);
        equipped = weakGun;
        bs = bullet.GetComponent<bulletscript>();
        bulletPool = new Stack<GameObject>();
        ammoCounter.text = "ammo: " + equipped.magazine + "/" + equipped.Ammo;
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
            ammoCounter.text = "ammo: " + equipped.magazine + "/" + equipped.Ammo;
            ps.pickupsScript.ammoPool.Push(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }

    void shoot(Gun gun)
    {
        //GameObject _bullet;
        //bulletscript _bs;
        //_bs = new bulletscript(gun.Speed,gameObject,gun.Range,gun.Damage);
        //_bs.speed =
        //Debug.Log(gun.Damage);
        if (gun.magazine == 0)
        {
            return;
        }
        if (nextFire < Time.time)
        {
            //bs.set(gun.Speed, gameObject, gun.Range, gun.Damage);
            //_bs = new bulletscript(gun.Speed, gameObject, gun.Range, gun.Damage);
            nextFire = Time.time + 1 / gun.FireRate;
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
            ammoCounter.text = "ammo: " + equipped.magazine + "/" + equipped.Ammo;
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
    // Update is called once per frame
    void Update()
    {
        

        if (equipped.magazine < equipped.ClipSize&&Input.GetKeyDown(KeyCode.R))
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
            
            ammoCounter.text = "ammo: " + equipped.magazine + "/" + equipped.Ammo;
            nextFire = Time.time + 1;
        }
        if (nextFire > Time.time)
        {
            //filled.transform.localScale = new Vector3(0.9f*(1-(nextFire-Time.time)), 0.9f*(1-(nextFire - Time.time)), 1);
            filled.transform.localScale = new Vector3(0.9f * (1-(nextFire - Time.time)*equipped.FireRate), 0.9f * (1- (nextFire - Time.time)*equipped.FireRate), 1);
        }
        else
        {
            filled.transform.localScale = new Vector3(0.9f, 0.9f, 1);
        }
        if (Input.GetMouseButton(0))
        {
            shoot(equipped);
            //Instantiate(bullet, transform.position,Quaternion.Euler(transform.rotation.x,transform.rotation.y, transform.rotation.z + Random.Range(-pistol.Spread/2,pistol.Spread/2)));
        }
    }
}
