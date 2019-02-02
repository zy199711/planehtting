using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class mapcontroller : MonoBehaviour
{
    // Start is called before the first frame update
    int hp, score;//游戏数据
    //ui显示
    Text hptext;
    Text scoretext;
    
    private float speed;//漂浮物飘动的速度
    private float enemytimelimit;//敌人生成间隔
    private int enemylimit;//一次生成敌人数量
    private Vector3 pos;//生成低人的位置
    private int leftenemy;//一次生成剩下的低人数
    public GameObject[] cloud;//云朵;
    public GameObject star;//星星
    public GameObject bg;//亮色背景
    public GameObject bg2;//暗色背景
    public GameObject enemy;
    private GameObject Map;//背景
    private GameObject[] floating;//漂浮物管理
    
    [HideInInspector]
    public GameObject Enamybulltes;
    [HideInInspector]
    public GameObject bulltes;
    private GameObject Enemys;
    private float lastenemy;
    int rows,clos;//地图大小
    public int Grows
    {
        get { return rows; }
    }
    public int Gclos
    {
        get { return clos; }
    }

    private int random,mod;//随机根
    List<Vector2> list;//坐标
    int floatinglimit;//漂浮物数量限制 
    //限制速度
    private float beftime;
    private float delaytime;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        //参数赋值
        hp = 40;score = 0;
        enemytimelimit = 5;
        enemylimit = 5;
        lastenemy = 0;
        beftime = 0;
        delaytime = 0.15f;
        floatinglimit = 5;
        random = 5;mod = 233337;
        floating = new GameObject[5];
        list = new List<Vector2>();
        
        rows = 10; clos = 18;
        speed = 0.1f;
        pos = new Vector3();
        Enamybulltes = new GameObject("Enamybulltes");
        bulltes = new GameObject("bulltes");
        Enemys = new GameObject("Enemys");
        Map = new GameObject("Map");
        initmap();
    }

    

    void Start()
    {
        hptext = GameObject.Find("hptext").GetComponent<Text>();
        scoretext = GameObject.Find("scoretext").GetComponent<Text>();
    }
    
    // Update is called once per frame
    void Update()
    {

        hptext.text = ""+hp;
        scoretext.text = "" + score;

        //控制时间
        if (Time.realtimeSinceStartup - beftime < delaytime) return;
        beftime = Time.realtimeSinceStartup;

        //控制敌人生成
        if (Time.realtimeSinceStartup - lastenemy > enemytimelimit)
        {
            int rd = getnext() % 23;
            if ((rd & 1) == 0)
            {
                pos.x = rd % clos;
                pos.y = rows;
            }
            else
            {
                pos.y = rd % rows;
                pos.x = clos;
            }
            //Debug.Log(pos.x + " "+pos.y);
            leftenemy = enemylimit;
            lastenemy = Time.realtimeSinceStartup;
        }

        if (leftenemy > 0)
        {
            leftenemy--;
            GameObject.Instantiate(enemy,pos,Quaternion.identity).transform.SetParent(Enemys.transform);
        }


        //控制漂浮物
        int ps = -1;
        for(int i = 0;i < floatinglimit; i++)
        {
            if (floating[i] != null)
            {
                floating[i].transform.Translate(Vector3.down * speed);
            }
            else ps = i;
        }
        if (ps != -1)
        {
            int op = getnext() % 6, gx = getnext() % (clos - 3) + 1;
            GameObject gm = GameObject.Instantiate(getsomething(op), new Vector2(gx,rows - 1 ), Quaternion.identity);
            gm.transform.SetParent(Map.transform);
            floating[ps] = gm;
            //Debug.Log(gx + "  " + gm.transform.position.y);
        }
        for (int i = 0; i < floatinglimit; i++)
        {
            if (floating[i]!= null && floating[i].transform.position.y < -1)
            {
                GameObject.Destroy(floating[i]);
                floating[i] = null;
            }
        }
    }
    //根据数字返回物体
    GameObject getsomething(int op)
    {
        if (op < 5) return cloud[op];
        else if (op == 5) return star;
        return null;
    }
    //初始化map
    void initmap()
    {

        for (int i = 1; i <= rows; i += 2)
        {
            for (int j = 1; j <= clos; j += 2)
            {
                if (j < clos - 2) list.Add(new Vector2(j, i));
                GameObject.Instantiate(bg2, new Vector2(j, i), Quaternion.identity).transform.SetParent(Map.transform);
            }
        }
        //生成漂浮物
        for (int i = 0; i < floatinglimit; i++)
        {
            int op = getnext() % 6, gx = getnext() % list.Count;
            GameObject gm = GameObject.Instantiate(getsomething(op), list[gx], Quaternion.identity);
            //Debug.Log(list[gx].x + " " + list[gx].y);
            gm.transform.SetParent(Map.transform);
            list.Remove(list[gx]);
            floating[i] = gm;
        }
    }
    //随机
    int getnext()
    {
        return random = random * 233 % mod;
    }

    void Getdamage(int x)
    {
        hp -= x;

        if (hp <= 0)
        {
            hp = 0;
            GameObject.Find("player").SendMessage("gameover");
        }
    }
    void Getscore(int x)
    {
        score += x;
    }
}
