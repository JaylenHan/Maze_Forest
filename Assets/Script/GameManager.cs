using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private AudioSource audioSource;
    public AudioClip Success;
    public AudioClip Failure;
    public MainCamera MainCamera;

    [Header("���� ���� �ð�")]
    public float playTime = 400;
    private float elapsedTime = 0;

    [Header("���� ���� ��Ȳ Ȯ��")]
    public bool isGameOver = true; // ���� ���� ����
    public bool isGameClear = false; // ���� ���� ����
    public Material defaultSkybox;
    public Material successSkybox;
    public GameObject fireWorks;

    [Header("�˾� UI")]
    [SerializeField]
    private GameObject RemainingTime;
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private GameObject Popup;
    private GameObject SuccessM;
    private GameObject Fail;
    [SerializeField]
    private GameObject restartButton2; //�»�� ����۹�ư
    private TextMeshProUGUI npcText;
    private TextMeshProUGUI failQuestionText;

    [Header("��ũ��Ʈ ���� ����")]
    public ScenarioEngine engine;
    private string script;

    [Header("������ ǥ���� ���")]
    public int FindNPC = 0;
    public int FailCount = 0; //������ ������ Ƚ��
    public CoroutineMovementNPC3[] npcs;
    public CoroutineMovementNPC2 mainNpc;

    [Header("���� ���� ��ư")]
    public GameObject GameLogo;
    public GameObject Btn_Start;
    public GameObject Btn_Setting;

    [Header("���� ���� ����")]
    public GameObject SettingPannel;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        //�˾�UI
        SuccessM = Popup.transform.Find("SuccessM").gameObject;
        Fail = Popup.transform.Find("Fail").gameObject;
        npcText = SuccessM.transform.Find("npc").GetComponent<TextMeshProUGUI>();
        failQuestionText = SuccessM.transform.Find("failQuestion").GetComponent<TextMeshProUGUI>();
    }


    IEnumerator ShowInfoUI()
    {
        RemainingTime.SetActive(true);
        yield return null;
    }

    void Update()
    {
        if (!isGameClear && !isGameOver)
        {
            elapsedTime += Time.deltaTime; //����ð�

            float remainingTime = GameManager.Instance.playTime - elapsedTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

            timeText.text = $"{minutes:D2}:{seconds:D2}";

            if (remainingTime <= 1)
            {
                isGameOver = true;
                GameOver();
            }

            // �ÿ� ����׿�. ���� ���� ���� �����ֱ� ���� (���ӽð� 15�� ����)
            if (Input.GetKeyDown(KeyCode.N))
            {
                elapsedTime += 285;
            }
        }
    }

    public void GameClear()
    {
        isGameClear = true;
        RemainingTime.SetActive(false);

        npcText.text = "��" + FindNPC + "���� NPC�� ã�Ҿ��!";

        if (FailCount == 0)
        {
            failQuestionText.text = "������ ��� �ѹ��� ������.\n���� �Ǹ��ؿ�!";
        }
        else
        {
            failQuestionText.text = "������ " + FailCount + "�� Ʋ�ȳ׿�.\n�������� �� �� ����غ���!";
        }

        for (int i = 0;i< npcs.Length;i++)
        {
            npcs[i].StopMovementAndPlayAnimation();
        }
        mainNpc.StopMovementAndPlayAnimation();

        SuccessM.SetActive(true);
        Fail.SetActive(false);
        Popup.SetActive(true);
        RenderSettings.skybox = successSkybox;
        fireWorks.SetActive(true);
    }

    private void GameOver()
    {
        isGameOver = true;

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        /* �˾� ����
         * ���� ���� ���� �˾��� ���� �ʰ� ����� ��ư�� Ȱ��ȭ
         */
        SuccessM.SetActive(false);
        Fail.SetActive(true);
        Popup.SetActive(true);

        // ���� ���н� �ó����� ���� ����
        engine.canvas.enabled = false;
        
        // �Ҳɳ��̴� �翬�� ����
        fireWorks.SetActive(false);
        Debug.Log("���� ����");

    }

    public void ClosePopup()
    {
       Popup.SetActive(false);
       restartButton2.SetActive(true);
    }
    public void RestartScene()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void Target_Success()
    {
        PlaySound(Success);
        script = Resources.Load<TextAsset>("Success").ToString();
        StartCoroutine(engine.PlayScript(script));
    }

    public void Target_Fail()
    {
        PlaySound(Failure);
        script = Resources.Load<TextAsset>("Fail").ToString();
        StartCoroutine(engine.PlayScript(script));
        FailCount++;
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void ShowBtnUI()
    {
        isGameOver = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        ShowBtn();
    }

    public void ShowBtn()
    {
        GameLogo.SetActive(true);
        Btn_Start.SetActive(true);
        Btn_Setting.SetActive(true);
    }

    public void HideBtn()
    {
        GameLogo.SetActive(false);
        Btn_Start.SetActive(false);
        Btn_Setting.SetActive(false);
    }

    public void Btn_GameStart()
    {
        isGameOver = false;
        StartCoroutine(DelayedModifyBlendSpeeds());
        HideBtn();
        mainNpc.RemoteNPCStart();
        StartCoroutine(ShowInfoUI());
        string script = Resources.Load<TextAsset>("Btn_GameStart").ToString();
        StartCoroutine(engine.PlayScript(script));
    }

    IEnumerator DelayedModifyBlendSpeeds()
    {
        yield return new WaitForSeconds(3);

        MainCamera.DelayedModifyBlendSpeed();
    }

    public void Btn_SettingPannel()
    {
        HideBtn();
        SettingPannel.SetActive(true); 
    }

    public void Btn_SettingPannelClose()
    {
        SettingPannel.SetActive(false);
        ShowBtn();
    }
}
