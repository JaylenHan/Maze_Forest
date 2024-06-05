using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾ ����� ����� �� ó��
public class ScriptBlock : MonoBehaviour
{
    public ScenarioEngine engine;
    public string filename;
    public string question;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            string script = Resources.Load<TextAsset>(filename).ToString();
            StartCoroutine(engine.PlayScript(script, question));
        }
    }
}
