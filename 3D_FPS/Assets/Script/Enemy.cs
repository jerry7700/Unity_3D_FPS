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
    public int addBullettime;

    private float timer;
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
        Tack();
        Fire();
        FacetoPlayer();
    }

    private void Tack()
    {
        nav.SetDestination(Player.position);

        if(nav.remainingDistance > rangeAttack)
        {
            anim.SetBool("跑步開關", true);
        }
        else
        {
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
            ManageBulletCount();
        }
        else
        {
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

    private IEnumerator AddBullet()
    {
        anim.SetTrigger("換彈觸發");
        yield return new WaitForSeconds(addBullettime);
        BulletCount += BulletClip;
    }
    private void FacetoPlayer()
    {
        Quaternion faceAngle = Quaternion.LookRotation(Player.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, faceAngle, Time.deltaTime * Fireface);
    }
    #endregion
}
