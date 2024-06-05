using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("�� ������Ʈ�� �����̸� true")]
    public bool Answer = true;
    public GameObject QuestionBox;
    public ThirdPersonController ThirdPersonController;

    private float moveSpeedSave = 0;
    private float sprintSpeedSave = 0;
    private bool Failed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            if (Answer) //O ���� ����
            {
                GameManager.Instance.Target_Success();
                QuestionBox.SetActive(false);
            }
            else // X ���� ����
            {
                if (!Failed) {
                    Failed = true;
                    GameManager.Instance.Target_Fail();
                    moveSpeedSave = ThirdPersonController.MoveSpeed;
                    sprintSpeedSave = ThirdPersonController.SprintSpeed;
                    ThirdPersonController.MoveSpeed = 0;
                    ThirdPersonController.SprintSpeed = 0;
                    Invoke("ReturnValue", 5);
                }
            }
        }
    }

    public void ReturnValue()
    {
        ThirdPersonController.MoveSpeed = moveSpeedSave;
        ThirdPersonController.SprintSpeed = sprintSpeedSave;
        Failed = false;
    }
}
