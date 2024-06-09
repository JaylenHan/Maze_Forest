using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 3.0f; //�Ѿ� �ı��ð�

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!(collision.gameObject.CompareTag("Player")))
        {
            Destroy(gameObject);
        }
        
    }
}
