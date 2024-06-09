using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private AudioSource audioSource;
    public AudioClip Success;
    public AudioClip Failure;

    [Header("���� ���� �ð�")]
    public float playTime = 400;
    private float elapsedTime = 0;

    [Header("���� ���� ��Ȳ Ȯ��")]
    public bool isGameOver = false; // ���� ���� ����
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

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        StartCoroutine(DelayedStartCoroutine());
        audioSource = gameObject.AddComponent<AudioSource>();

        //�˾�UI
        SuccessM = Popup.transform.Find("SuccessM").gameObject;
        Fail = Popup.transform.Find("Fail").gameObject;
        npcText = SuccessM.transform.Find("npc").GetComponent<TextMeshProUGUI>();
        failQuestionText = SuccessM.transform.Find("failQuestion").GetComponent<TextMeshProUGUI>();
    }

    IEnumerator DelayedStartCoroutine()
    {
        yield return new WaitForSeconds(10);
        StartCoroutine(ShowInfoUI());
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
}
