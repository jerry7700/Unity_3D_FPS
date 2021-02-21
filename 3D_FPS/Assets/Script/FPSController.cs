using UnityEngine;

public class FPSController : MonoBehaviour
{
    #region 基本欄位
    [Header("移動速度"), Range(0, 2000)]
    public float Speed;
    [Header("視角靈敏度"), Range(0, 2000)]
    public float turn;
    [Header("跳躍"), Range(0, 2000)]
    public float jump;
    [Header("地板偵測位移")]
    public Vector3 floorOffset;
    [Header("地板偵測半徑"), Range(0, 20)]
    public float floorRadius = 1;

    private Animator anim;
    private Rigidbody rb;
    #endregion

    #region 子彈欄位
    [Header("生成子彈位置")]
    public Transform firePoint;
    [Header("子彈")]
    public GameObject Bullet;
    [Header("子彈速度")]
    public float BulletSpeed;
    [Header("子彈彈夾")]
    public int BulletCurrent;
    [Header("總子彈")]
    public int BulletTotal;
    #endregion

    /// <summary>
    /// 第一次執行，使用一次 Awake
    /// </summary>
    private void Awake()
    {
        Cursor.visible = false;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    /// <summary>
    /// 地板球體
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position + floorOffset, floorRadius);
    }

    private void Update()
    {
        Move();
        Jump();
        Fire();
    }
    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        float v = Input.GetAxis("Vertical"); //前後值
        float h = Input.GetAxis("Horizontal");

        rb.MovePosition(transform.position + transform.forward * v * Speed * Time.deltaTime + transform.right * h * Speed * Time.deltaTime);

        float x = Input.GetAxis("Mouse X");
        transform.Rotate(0, x * Time.deltaTime * turn, 0);
    }
    /// <summary>
    /// 跳躍
    /// </summary>
    private void Jump()
    {
        // 3D 模式物理碰撞偵測
        //物理.覆蓋球體(中心點 + 位移, 半徑, 1 >> 8)
        Collider[] hits = Physics.OverlapSphere(transform.position + floorOffset, floorRadius, 1 << 8);

        if(hits.Length > 0 && hits[0] && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(0, jump, 0);
        }
    }
    /// <summary>
    /// 開槍
    /// </summary>
    private void Fire()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject temp = Instantiate(Bullet, firePoint.position, firePoint.rotation);
            temp.GetComponent<Rigidbody>().AddForce(-firePoint.forward * BulletSpeed);
        }
    }
}
