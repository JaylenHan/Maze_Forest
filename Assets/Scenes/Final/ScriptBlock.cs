using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾ ����� ����� �� ó��
public class ScriptBlock : MonoBehaviour
{
    public ScenarioEngine engine;
    public string filename;
    private void OnTriggerEnter(Collider other)
    {
        //if�� �÷��̾� ���� üũ(�±� ���)
        string script = Resources.Load<TextAsset>(filename).ToString();
        StartCoroutine(engine.PlayScript(script));
    }
}
