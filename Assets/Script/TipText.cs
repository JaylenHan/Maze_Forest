using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;

public class TipText : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    private string[] TipTexts = new string[] { "ù��° ���Դϴ�.",
        "�ι�° ���Դϴ�.",
        "����° ���Դϴ�.",
        "�׹�° ���Դϴ�."};
    private int RandomNum;

    void OnEnable()
    {
        RandomNum = Random.Range(0, 3);
    }

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = TipTexts[RandomNum];
    }



    // Update is called once per frame
    void Update()
    {

    }
}
