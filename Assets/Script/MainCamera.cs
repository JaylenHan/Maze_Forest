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
    }

    public void DelayedModifyBlendSpeed()
    {
        cinemachineBrain.m_DefaultBlend.m_Time = 0.3f;
    }
}
