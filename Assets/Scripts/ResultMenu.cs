using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultMenu : MonoBehaviour
{
    [SerializeField] GameObject UI;
    //판정 텍스트들 넣어줌
    [SerializeField] Text[] txtCount;
    [SerializeField] Text txtCoin;
    [SerializeField] Text txtScore;
    [SerializeField] Text txtMaxCombo;

    ScoreManager scoreManager;
    ComboManager comboManager;
    TimingManager timingManager;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        comboManager = FindObjectOfType<ComboManager>();
        timingManager = FindObjectOfType<TimingManager>();
    }

    public void showResult()
    {
        UI.SetActive(true);

        for (int i = 0; i < txtCount.Length; i++)
            txtCount[i].text = "0";

        txtCoin.text = "0";
        txtMaxCombo.text = "0";

        int[] judge = timingManager.GetJudgeRecord();
        int currentScore = scoreManager.GetCurrentScore();
        int maxCombo = comboManager.GetMaxCombo();
        int coin = currentScore / 50;

        for (int i = 0; i < txtCount.Length; i++)
        {
            txtCount[i].text = judge[i].ToString();
        }

        txtScore.text = string.Format("{0:#,##0}", currentScore);
        txtMaxCombo.text = string.Format("{0:#,##0}", maxCombo);
        txtCoin.text = string.Format("{0:#,##0}", coin);
    }
}
