using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾ ����� ����� �� ó��
public class GameClear : MonoBehaviour
{
    public ScenarioEngine engine;
    public AudioClip congBgm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.PlaySound(congBgm);
            GameManager.Instance.GameClear();
            string script = Resources.Load<TextAsset>("GameClear").ToString();
            StartCoroutine(engine.PlayScript(script));
        }
    }
}
