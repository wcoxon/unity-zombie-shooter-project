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
    public Gun(float fireRate,float spread,float range,float speed,float recoil,int bullets,float damage)
    {
        FireRate = fireRate;
        Spread = spread;
        Range = range;
        Speed = speed;
        Recoil = recoil;
        Bullets = bullets;
        Damage = damage;
    }
}

public class guns : MonoBehaviour
{
    public Stack<GameObject> bulletPool;
    public GameObject bullet;
    public Gun machinegun;
    public float nextFire;
    Gun pistol;
    public Gun equipped;
    public GameObject filled;
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
        machinegun = new Gun(15, 5, 30, 19,3,1,5);
        pistol = new Gun(100, 100, 100, 10, 100, 5, 10);
        equipped = machinegun;
        bs = bullet.GetComponent<bulletscript>();
        bulletPool = new Stack<GameObject>();
        //bs = new bulletscript();
        //bs.speed = 
        //pistol = new Gun(gunstuff[0], gunstuff[1], gunstuff[2], gunstuff[3], gunstuff[4], (int)gunstuff[5],gunstuff[6]);
        //equipped = pistol;
    }
    
    void shoot(Gun gun)
    {
        //GameObject _bullet;
        //bulletscript _bs;
        //_bs = new bulletscript(gun.Speed,gameObject,gun.Range,gun.Damage);
        //_bs.speed =
        if (nextFire < Time.time)
        {
            bs.set(gun.Speed, gameObject, gun.Range, gun.Damage);
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
    // Update is called once per frame
    void Update()
    {
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
