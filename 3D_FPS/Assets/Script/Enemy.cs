using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    #region 欄位
    /// <summary>
    /// 玩家座標
    /// </summary>
    private Transform Player;

    /// <summary>
    /// 代理器
    /// </summary>
    private NavMeshAgent nav;

    private Animator anim;

    [Header("速度"), Range(0f, 30f)]
    public float speed = 2.5f;

    [Header("攻擊範圍"),Range(2 , 100)]
    public float rangeAttack = 5f;

    [Header("生成子彈位置")]
    public Transform Point;
    [Header("子彈")]
    public GameObject Bullet;
    [Header("子彈速度")]
    public float BulletSpeed;
    [Header("開槍間隔時間"), Range(0f, 5f)]
    public float Interval;
    [Header("開槍轉向"), Range(0f, 100f)]
    public float Fireface;
    [Header("彈夾目前數量")]
    public int BulletCount = 30;
    [Header("彈夾數量")]
    public int BulletClip = 30;
    [Header("補充子彈時間"), Range(0, 5)]
    public float addBullettime;
    [Header("血量")]
    public float HP = 100;
    [Header("攻擊力")]
    public int bulletAttack =5;

    private float timer;
    private bool isBullet;
    #endregion

    #region 方法
    /// <summary>
    /// 在開始使用一次
    /// </summary>
    private void Awake()
    {
        Player = GameObject.Find("玩家").transform;
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        nav.speed = speed;
        nav.stoppingDistance = rangeAttack;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position,rangeAttack);
    }

    private void Update()
    {
        if (isBullet) return;
        Tack();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Tack()
    {
        nav.SetDestination(Player.position);

        if(nav.remainingDistance > rangeAttack)
        {
            anim.SetBool("跑步開關", true);
        }
        else
        {
            Fire();
            anim.SetBool("跑步開關", false);
        }
    }

    /// <summary>
    /// 開槍
    /// </summary>
    private void Fire()
    {
        if(timer >= Interval)
        {
            timer = 0;
            GameObject temp = Instantiate(Bullet, Point.position, Point.rotation);
            temp.GetComponent<Rigidbody>().AddForce(-Point.right * BulletSpeed);
            temp.GetComponent<Bullet>().attack = bulletAttack;
            ManageBulletCount();
        }
        else
        {
            FacetoPlayer();
            timer += Time.deltaTime;
        }
    }

    /// <summary>
    /// 管理子彈數量
    /// </summary>
    private void ManageBulletCount()
    {
        BulletCount--;

        if(BulletCount < 0)
        {
            StartCoroutine(AddBullet());
        }
    }

    /// <summary>
    /// 延遲換彈
    /// </summary>
    /// <returns></returns>
    private IEnumerator AddBullet()
    {
        anim.SetTrigger("換彈觸發");
        isBullet = true;
        yield return new WaitForSeconds(addBullettime);
        isBullet = false;
        BulletCount += BulletClip;
    }

    /// <summary>
    /// 面向玩家
    /// </summary>
    private void FacetoPlayer()
    {
        Quaternion faceAngle = Quaternion.LookRotation(Player.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, faceAngle, Time.deltaTime * Fireface);
    }

    /// <summary>
    /// 受傷
    /// </summary>
    /// <param name="getDamage"></param>
    private void Damage(float getDamage)
    {
        HP -= getDamage;
        if (HP <= 0) Dath();
    }
    
    /// <summary>
    /// 死亡
    /// </summary>
    private void Dath()
    {
        anim.SetTrigger("死亡觸發");
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        this.enabled = false;

    }

    /// <summary>
    /// 碰撞開始執行一次
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "子彈")
        {
            float damage = collision.gameObject.GetComponent<Bullet>().attack;
            
            if (collision.contacts[0].thisCollider.GetType().Equals(typeof(SphereCollider)))
            {
                print("暴頭");
                Damage(100);
            }
            else
            {
                Damage(damage);
            }
        }
    }
    #endregion
}
