using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemycontroller : MonoBehaviour
{

    public GameObject bullte1;
    private GameObject Enamybulltes;

    // Start is called before the first frame update
    private float speed;//速度
    private Vector3 velocity;
    //限制速度
    private float beftime;
    private float delaytime;
    private float bulltelimittime;
    private float begintime;
    int rows, clos;//行列
    //限制bullte射速
    private float lastbullte;
    private bool isdestroy;
    int hp;
    private void Awake()
    {
        begintime = Time.realtimeSinceStartup;
        hp = 2;
        bulltelimittime = 2;
        isdestroy = false;
        delaytime = 1.0f/35;
        speed = 0.2f;
        velocity = new Vector3();

    }
    void Start()
    {
        //得到速度向量
        
        mapcontroller temp = GameObject.Find("Mapmanager").GetComponent<mapcontroller>();
        rows = temp.Grows;clos = temp.Gclos;
        Vector3 des = new Vector3(clos, rows, 0);
        des = des - gameObject.transform.position;

        velocity = (des - gameObject.transform.position) * speed / Vector3.Distance(des, gameObject.transform.position);

        Enamybulltes = GameObject.Find("Mapmanager").GetComponent<mapcontroller>().Enamybulltes;
            

        
    }

    // Update is called once per frame
    void Update()
    {
        //控制攻击
        if(gameObject.transform.position.y > clos/3f && Time.realtimeSinceStartup - lastbullte > bulltelimittime)
        {
            fire();
            lastbullte = Time.realtimeSinceStartup;
        }

        if (isdestroy)
        {
            //把动画播放完
            if (Time.realtimeSinceStartup - beftime >= 0.6f) {  GameObject.Destroy(gameObject); }
            return;
        }
        if (Time.realtimeSinceStartup - beftime <= delaytime) return;
        gameObject.transform.Translate(velocity);
        beftime = Time.realtimeSinceStartup;
        //Debug.Log("enemy " + gameObject.transform.position.x);
        if (gameObject.transform.position.x < -10
            || gameObject.transform.position.x > clos + 10
                || gameObject.transform.position.y < -10
                    || gameObject.transform.position.y > rows + 10)
            GameObject.Destroy(gameObject);



    }

    void Getdestroy(int x)
    {
        hp -= x;
        if (hp > 0) return;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<Animator>().SetTrigger("destroy");
        isdestroy = true;
    }

    void fire()
    {
        GameObject.Instantiate(bullte1, gameObject.transform.position, Quaternion.identity).transform.SetParent(Enamybulltes.transform);
    }
}
