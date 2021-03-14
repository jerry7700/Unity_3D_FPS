using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSController : MonoBehaviour
{
    #region 基本欄位
    [Header("移動速度"), Range(0, 2000)]
    public float Speed;
    [Header("視角靈敏度"), Range(0, 2000)]
    public float turn;
    [Header("跳躍"), Range(0, 10000)]
    public float jump;
    [Header("地板偵測位移")]
    public Vector3 floorOffset;
    [Header("地板偵測半徑"), Range(0, 20)]
    public float floorRadius = 1;
    [Header("血量與血條")]
    public Text textHp;
    public Image imageHp;

    private Transform traMain;
    private Transform traCam;
    private float HP = 100;
    private float HPmax = 100;
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
    [Header("補充彈夾")]
    public int Bulletclip;
    [Header("總子彈")]
    public int BulletTotal;
    [Header("文字:子彈彈夾")]
    public Text textBulletCurrent;
    [Header("文字:總子彈")]
    public Text textBulletTotal;
    [Header("補充子彈時間"), Range(0, 5)]
    public int addBullettime;
    [Header("開槍音效")]
    public AudioClip SoundFire;
    [Header("換彈音效")]
    public AudioClip SoundAddBullet;
    [Header("開槍間隔時間"), Range(0f, 1f)]
    public float fireInterval;
    [Header("攻擊力")]
    public int bulletAttack;

    private AudioSource aud;
    private float timer;

    private bool isAddBullet;
    #endregion

    /// <summary>
    /// 第一次執行，使用一次 Awake
    /// </summary>
    private void Awake()
    {
        Cursor.visible = false;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        aud = GetComponent<AudioSource>();

        traMain = transform.Find("Main Camera");
        traCam = transform.Find("Camera");
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
        AddBullet();
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

        if (hits.Length > 0 && hits[0] && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(0, jump, 0);
        }
    }
    /// <summary>
    /// 開槍
    /// </summary>
    private void Fire()
    {
        if (Input.GetKey(KeyCode.Mouse0) && BulletCurrent > 0 && !isAddBullet)
        {
            if(timer >= fireInterval)
            {
                anim.SetTrigger("開槍觸發");
                timer = 0;
                aud.PlayOneShot(SoundFire, Random.Range(0.5f, 0.5f));

                BulletCurrent--;
                textBulletCurrent.text = BulletCurrent.ToString();
                GameObject temp = Instantiate(Bullet, firePoint.position, firePoint.rotation);
                temp.GetComponent<Rigidbody>().AddForce(-firePoint.forward * BulletSpeed);
                temp.GetComponent<Bullet>().attack = bulletAttack;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    private void AddBullet()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isAddBullet && BulletTotal > 0 && BulletCurrent < Bulletclip)
        {
            StartCoroutine(AddBulletDelay());
        }
    }

    private IEnumerator AddBulletDelay()
    {
        anim.SetTrigger("換彈觸發");
        aud.PlayOneShot(SoundAddBullet, Random.Range(0.5f, 0.5f));
        isAddBullet = true;
        yield return new WaitForSeconds(addBullettime);
        isAddBullet = false;
        int add = Bulletclip - BulletCurrent;
        if (BulletTotal >= add)
        {
            BulletCurrent += add;
            BulletTotal -= add;
        }
        else
        {
            BulletCurrent += BulletTotal;
            BulletTotal = 0;
        }
        textBulletCurrent.text = BulletCurrent.ToString();
        textBulletTotal.text = BulletTotal.ToString();
    }

    /// <summary>
    /// 受傷
    /// </summary>
    /// <param name="getDamage"></param>
    private void Damage(float getDamage)
    {
        if (HP <= 0) return;

        HP -= getDamage;
 
        if (HP <= 0) Dath();

        textHp.text = HP.ToString();
        imageHp.fillAmount = HP / HPmax;
    }

    /// <summary>
    /// 死亡
    /// </summary>
    private void Dath()
    {
        HP = 0;
        anim.SetTrigger("死亡觸發");
        this.enabled = false;
        StartCoroutine(MoveCamera());
    }

    /// <summary>
    /// 攝影機位移
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveCamera()
    {
        traMain.LookAt(transform);
        traCam.LookAt(transform);

        Vector3 posCam = traMain.position;

        float yCam = posCam.y;
        float zCam = posCam.z;
        float xCam = posCam.x;
        float upCam = yCam + 3;
        float lCam = xCam - 1.5f;
        float backCam = zCam + 0.8f;

        while(yCam < upCam)
        {
            yCam += 0.05f;
            if (xCam > lCam) xCam -= 0.1f;
            if (zCam < backCam) zCam += 0.1f;
            posCam.y = yCam;
            posCam.z = zCam;
            posCam.x = xCam;

            traMain.position = posCam;
            traCam.position = posCam;

            yield return new WaitForSeconds(0.05f);
        }
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
            Damage(damage);
        }
    }
}
