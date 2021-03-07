using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float attack;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
