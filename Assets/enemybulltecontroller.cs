using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemybulltecontroller : MonoBehaviour
{
    public GameObject boom;
    //控制射速
    private float beftime;
    private float delaytime;
    private float speed;
    int rows, clos;
    private Vector3 velocity;//速度方向

    private void Awake()
    {
        beftime = 0;
        delaytime = 1.0f / 30;
        speed = 0.25f;
    }
    // Start is called before the first frame update
    void Start()
    {
        mapcontroller temp = GameObject.Find("Mapmanager").GetComponent<mapcontroller>();
        rows = temp.Grows; clos = temp.Gclos;
        
        GameObject player = GameObject.Find("player");
        Vector3 des = player.transform.position;
        velocity = (des - gameObject.transform.position) * speed / Vector3.Distance(des, gameObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup - beftime <= delaytime) return;
        gameObject.transform.Translate(velocity);
        beftime = Time.realtimeSinceStartup;
        //Debug.Log("enemy " + gameObject.transform.position.x);
        if (gameObject.transform.position.x < -2
            || gameObject.transform.position.x > clos + 5
                || gameObject.transform.position.y < -2
                    || gameObject.transform.position.y > rows + 2)
            GameObject.Destroy(gameObject);
    }
    void Getdestroy()
    {
        GameObject.Destroy(gameObject);
    }
}
