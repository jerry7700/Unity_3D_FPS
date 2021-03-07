using UnityEngine;
using UnityEngine.AI;

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
    }

    public void Tack()
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

    public void Fire()
    {
        if(timer >= Interval)
        {
            timer = 0;
            Instantiate(Bullet, Point.position, Point.rotation);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
    #endregion
}
