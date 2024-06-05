using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float playTime = 400;
    private float elapsedTime = 0;

    public bool isGameOver = false; // ���� ���� ����

    [SerializeField]
    private TextMeshProUGUI timeText;
    // Start is called before the first frame update


    [Header("��ũ��Ʈ ���� ����")]
    public ScenarioEngine engine;
    private string script;


    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime; //����ð�

        float remainingTime = GameManager.Instance.playTime - elapsedTime;
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        timeText.text = $"{minutes:D2}:{seconds:D2}";

        if (remainingTime <= 0)
        {
            isGameOver = true;
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        Debug.Log("���� ����");
    }


    public void Target_Success()
    {
        script = Resources.Load<TextAsset>("Success").ToString();
        StartCoroutine(engine.PlayScript(script));
    }

    public void Target_Fail()
    {
        script = Resources.Load<TextAsset>("Fail").ToString();
        StartCoroutine(engine.PlayScript(script));
    }
}
