using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public StarterAssets.StarterAssetsInputs input;
    public Animator animator;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;

    public GameObject uiCanvas;  // Canvas�� �����ϴ� ����
    public CinemachineVirtualCamera mainCamera;
    public CinemachineVirtualCamera attackCamera;

    private bool isAttackingIdle = false;
    private Camera mainCameraComponent;

    [Header("Aim")]
    [SerializeField]
    private GameObject aimObj;
    [SerializeField]
    private float aimObjDis = 10f;
    [SerializeField]
    private LayerMask targetLayer;

    private void Awake()
    {
        mainCameraComponent = Camera.main;
    }

    private void Update()
    {
        HandleAttackAndFire();
    }

    private void HandleAttackAndFire()
    {
        if (input.attack)
        {
            if (!isAttackingIdle)
            {
                // AttackIdle �ִϸ��̼��� �����մϴ�.
                animator.SetBool("IsAttackingIdle", true);
                isAttackingIdle = true;

                // UI Canvas Ȱ��ȭ
                uiCanvas.SetActive(true);

                // �ó׸ӽ� ī�޶� ��ȯ
                mainCamera.Priority = 0;
                attackCamera.Priority = 1;
            }

            if (input.fire)
            {
                animator.SetTrigger("Attack");
                FireBullet();
                input.fire = false;
            }
        }
        else
        {
            if (isAttackingIdle)
            {
                // AttackIdle �ִϸ��̼��� �����մϴ�.
                animator.SetBool("IsAttackingIdle", false);
                isAttackingIdle = false;

                // UI Canvas ��Ȱ��ȭ
                uiCanvas.SetActive(false);

                // �ó׸ӽ� ī�޶� ��ȯ
                mainCamera.Priority = 1;
                attackCamera.Priority = 0;
            }
        }
    }

    private void FireBullet()
    {
        Vector3 targetPosition = Vector3.zero;
        Transform camTransform = mainCameraComponent.transform;
        RaycastHit hit;

        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, Mathf.Infinity, targetLayer, QueryTriggerInteraction.Ignore))
        {
            targetPosition = hit.point;
            aimObj.transform.position = hit.point;
        }
        else
        {
            targetPosition = camTransform.position + camTransform.forward * aimObjDis;
            aimObj.transform.position = camTransform.position + camTransform.forward * aimObjDis;
        }

        Vector3 direction = (targetPosition - firePoint.position).normalized;

        // �÷��̾ Ÿ�� �������� ȸ����ŵ�ϴ�.
        Vector3 lookDirection = new Vector3(direction.x, 0, direction.z);
        transform.rotation = Quaternion.LookRotation(lookDirection);

        // �Ѿ��� �����ϰ� �߻��մϴ�.
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(direction * bulletSpeed, ForceMode.Impulse);
    }
}
