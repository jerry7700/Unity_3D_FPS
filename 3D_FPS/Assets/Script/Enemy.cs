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

    [Header("速度"), Range(0f, 30f)]
    public float speed = 2.5f;

    [Header("攻擊範圍"),Range(2 , 100)]
    public float rangeAttack = 5f;
    #endregion

    #region 方法
    /// <summary>
    /// 在開始使用一次
    /// </summary>
    private void Awake()
    {
        Player = GameObject.Find("玩家").transform;
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
    }

    public void Tack()
    {
        nav.SetDestination(Player.position);
    }
    #endregion
}
