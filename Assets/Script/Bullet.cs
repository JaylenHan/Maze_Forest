using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 3.0f; // �Ѿ��� 3�� �Ŀ� ��������� ����

    private void Start()
    {
        // ���� �ð� �Ŀ� �Ѿ��� �ı�
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �浹 �� �Ѿ��� �ı�
        if (!(collision.gameObject.CompareTag("Player")))
        {
            Destroy(gameObject);
        }
        
    }
}
