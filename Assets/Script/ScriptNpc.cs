using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾ ����� ����� �� ó��
public class ScriptNpc : MonoBehaviour
{
    public ScenarioEngine engine;
    public string filename;
    public bool justOneCheck = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && justOneCheck == false)
        {
            GameManager.Instance.FindNPC++; //�������� ã�� NPC���� ǥ���ϱ� ����
            string script = Resources.Load<TextAsset>(filename).ToString();
            StartCoroutine(engine.PlayScript(script));
            justOneCheck = true;
        }
    }
}
