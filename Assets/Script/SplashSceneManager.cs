using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject canvasA;
    [SerializeField]
    private GameObject canvasB;
    [SerializeField]
    private string nextSceneName;
    [SerializeField]
    private float delayBetweenCanvases = 4.0f;

    private AsyncOperation asyncLoad;

    private void Start()
    {
        canvasA.SetActive(true);
        canvasB.SetActive(false);

        Cursor.visible = false; //���÷��� ���콺 ��Ȱ��ȭ
        StartCoroutine(LoadSceneAsync()); // �񵿱� �� �ε带 �����մϴ�.

        // ĵ���� ��ȯ�� �����մϴ�.
        StartCoroutine(SwitchCanvasAndLoadScene());
    }

    private IEnumerator LoadSceneAsync()
    {
        asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        asyncLoad.allowSceneActivation = false; 
        yield return asyncLoad;
    }

    private IEnumerator SwitchCanvasAndLoadScene()
    {
        yield return new WaitForSeconds(delayBetweenCanvases);

        canvasA.SetActive(false);
        canvasB.SetActive(true);

        yield return new WaitForSeconds(delayBetweenCanvases);

        Cursor.visible = true; //���÷��� ���� �� ���콺 Ȱ��ȭ
        asyncLoad.allowSceneActivation = true;
    }
}
