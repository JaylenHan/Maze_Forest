using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ��Ʈ�� ���� �ӵ� ����
 */

public class MainCamera : MonoBehaviour
{
    CinemachineBrain cinemachineBrain;
    void Start()
    {
        cinemachineBrain = GetComponent<CinemachineBrain>();
        cinemachineBrain.m_DefaultBlend.m_Time = 3f;

        StartCoroutine(DelayedModifyBlendSpeed());
    }

    IEnumerator DelayedModifyBlendSpeed()
    {
        yield return new WaitForSeconds(10);

        cinemachineBrain.m_DefaultBlend.m_Time = 0.3f;
    }
}
