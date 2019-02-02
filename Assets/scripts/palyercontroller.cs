using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class palyercontroller : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bullte1;
    private GameObject bulltes;
    private GameObject mapmanager;
    //限制速度
    private float beftime;
    private float delaytime;
    private float speed;
    private float bulltelimittime;
    //限制bullte射速
    private float lastbullte;

   private bool isdestroy;

    private void Awake()
    {
        isdestroy = false;
        beftime = 0;
        delaytime = 1.0f / 60;
        speed = 0.15f;
        lastbullte = 0;
        bulltelimittime = 1.0f/10 ;
    }
    void Start()
    {
        mapmanager = GameObject.Find("Mapmanager");
        bulltes = mapmanager.GetComponent<mapcontroller>().bulltes;
    }

    // Update is called once per frame

    void Update()
    {
        if (isdestroy)
        {
            return;
        }

        if (Input.GetKey("k")&&Time.realtimeSinceStartup - lastbullte>=bulltelimittime)
        {
            //Debug.Log(Time.realtimeSinceStartup - lastbullte);
            GameObject abullte = GameObject.Instantiate(bullte1, 
                gameObject.transform.position+(Vector3.up), Quaternion.identity);
            abullte.transform.SetParent(bulltes.transform);
            lastbullte = Time.realtimeSinceStartup;
        }


        //检查物体撞击
        float lx, ly;
        lx = gameObject.GetComponent<BoxCollider2D>().size.x / 2 + 0.1f;
        ly = gameObject.GetComponent<BoxCollider2D>().size.y / 2 + 0.1f;

        Getcollider(new Vector2(lx, -ly), new Vector2(lx, ly));
        Getcollider(new Vector2(lx, ly), new Vector2(-lx, ly));
        Getcollider(new Vector2(-lx, ly), new Vector2(-lx, -ly));
        Getcollider(new Vector2(-lx, -ly), new Vector2(lx, -ly));



        if (Time.realtimeSinceStartup - beftime <= delaytime) return;

        int dir = getdir();
        if (dir == 0) return;

        //检测碰撞
        RaycastHit2D hit = Physics2D.Linecast(
            gameObject.transform.position + (Vector3)getvec(dir) * (0.5f+speed) + 0.5f * getvec3(dir),
                gameObject.transform.position + (Vector3)getvec(dir) * (0.5f + speed) - 0.5f * getvec3(dir));

        if (hit.collider == null)
        {
            gameObject.transform.Translate(getvec(dir) * speed);
        }
        else
        {
            //Debug.Log(hit.collider.tag);
            switch (hit.collider.tag)
            {
                case "bg": break;
                case "bullte": gameObject.transform.Translate(getvec(dir) * speed);break;
                case "enemy": gameObject.transform.Translate(getvec(dir) * speed); break;
            }
        }
        //Debug.Log(Time.realtimeSinceStartup - beftime);
        //gameObject.transform.Translate(getvec(dir) * speed);
        beftime = Time.realtimeSinceStartup;



    }
    //得到方向
    int getdir()
    {
        if (Input.GetKey("d")) return 1;
        if (Input.GetKey("w")) return 2;
        if (Input.GetKey("a")) return 3;
        if (Input.GetKey("s")) return 4;
        return 0;
    }
    //得到向量
    Vector2 getvec(int dir)
    {
        if (dir == 1) return Vector2.right;
        if (dir == 2) return Vector2.up;
        if (dir == 3) return Vector2.left;
        if (dir == 4) return Vector2.down;
        return Vector2.zero;
    }
    //得到向量的垂直向量
    Vector3 getvec3(int dir)
    {
        if (dir == 1 || dir == 3) return Vector2.up;
        if (dir == 1 || dir == 3) return Vector2.left;
        return Vector3.zero;
    }


    //查看是否有物体存在
    void Getcollider(Vector3 vec1, Vector3 vec2)
    {

        RaycastHit2D hit = Physics2D.Linecast(gameObject.transform.position + vec1,
            gameObject.transform.position + vec2);
        if (hit.collider != null)
        {
            //Debug.Log("Getcollider " + hit.collider.tag);
            switch (hit.collider.tag)
            {
                case "wall": break;
                case "player":  break;  //需要增加向player发送信息然后销毁
                case "bullte": break;
                case "enemy": hit.collider.SendMessage("Getdestroy",10000); mapmanager.SendMessage("Getdamage", 2); break;
                case "enemybullte": hit.collider.SendMessage("Getdestroy");mapmanager.SendMessage("Getdamage", 1); break;
            }
        }


    }
    void gameover()
    {
        isdestroy = true;
        gameObject.GetComponent<Animator>().SetTrigger("destroy");
    }
    
}
