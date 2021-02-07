using UnityEngine;

public class FPSController : MonoBehaviour
{
    [Header("移動速度"), Range(0, 2000)]
    public float Speed;
    [Header("視角靈敏度"), Range(0, 2000)]
    public float turn;

    private Animator anim;
    private Rigidbody rb;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float v = Input.GetAxis("Vertical"); //前後值
        float h = Input.GetAxis("Horizontal");

        rb.MovePosition(transform.position + transform.forward * v * Speed * Time.deltaTime + transform.right * h * Speed * Time.deltaTime);
    }
}
