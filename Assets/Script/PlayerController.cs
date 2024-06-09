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
    public ScenarioEngine engine;

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

        string script = Resources.Load<TextAsset>("GameStart").ToString();
        StartCoroutine(engine.PlayScript(script));
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOver)
        {
            return;
        }

        //�⺻ ����
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //LeftALT Ŭ������ ���콺 Ȱ��ȭ
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        HandleAttackAndFire();
        HandleMovement();
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
                // ���� Ʈ���� Attack ���·� ����
                animator.SetFloat("AttackSpeed", 1f);
                FireBullet();
                input.fire = false;
            }
        }
        else
        {
            if (isAttackingIdle)
            {
                // Attack ���¸� �����ϰ� ���� Ʈ���� AttackIdle ���·� ��ȯ
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

    private void HandleMovement()
    {
        bool isWalking = input.move != Vector2.zero;

        if (isAttackingIdle)
        {
            // AttackIdle ���¿��� �̵��� �� AttackWalk ���·� ��ȯ
            animator.SetFloat("AttackSpeed", isWalking ? 0.5f : 0f);
        }
        else
        {
            // Idle Walk Run Blend ������ �̵� �ִϸ��̼� ó��
            animator.SetBool("isWalking", isWalking);
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
