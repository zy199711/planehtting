using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulltecontroller : MonoBehaviour
{
    public GameObject boom;
    private GameObject mapmanager;
    private float beftime;
    private float delaytime;
    private float speed;
    private bool isdestroy;
    int rows, clos;
    private void Awake()
    {
        isdestroy = false;
        beftime = 0;
        delaytime = 1.0f / 60;
        speed = 0.5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        mapmanager = GameObject.Find("Mapmanager");
        mapcontroller temp = mapmanager.GetComponent<mapcontroller>();
        rows = temp.Grows; clos = temp.Gclos;
    }

    // Update is called once per frame
    void Update()
    {
        if (isdestroy)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = boom.GetComponent<SpriteRenderer>().sprite;
            if (Time.realtimeSinceStartup - beftime >= delaytime * 5) GameObject.Destroy(gameObject);
            return;
        }

        if (Time.realtimeSinceStartup - beftime <= delaytime) return;
        beftime = Time.realtimeSinceStartup;
        int dir = 2;
        //检测碰撞
        RaycastHit2D hit = Physics2D.Linecast(
            gameObject.transform.position + (Vector3)getvec(dir) * (0.25f + speed) + 0.25f * getvec3(dir),
                gameObject.transform.position + (Vector3)getvec(dir) * (0.25f + speed) - 0.25f * getvec3(dir));

        if(hit.collider == null)
        {
            gameObject.transform.Translate(Vector2.up * speed);
        }
        else
        {
            //Debug.Log(hit.collider.tag);
            switch (hit.collider.tag)
            {
                case "bg": Getbg(); break;
                case "enemy" :hit.collider.SendMessage("Getdestroy",1);isdestroy = true; mapmanager.SendMessage("Getscore", 100); break;
                default: gameObject.transform.Translate(Vector2.up * speed); break;
            }
        }
        
        
    }
    void Getbg()
    {
        //保证撞到墙是在墙上销毁
        Vector3 des = gameObject.transform.position;
        des.y = rows;
        gameObject.transform.Translate(des - gameObject.transform.position);
        isdestroy = true;
        
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
}
