using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    #region 스코어
    [SerializeField] Text txtScore;
    [SerializeField] int increaseScore = 10;

    int CurrentScore = 0;

    //판정 따라서 점수 가중치. 
    [SerializeField] float[] weight;
    [SerializeField] int ComboBonusScore = 10;
    #endregion

    Animator anim;
    string ScoreUp = "ScoreUp";

    ComboManager comboManager;

    void Start()
    {
        anim = GetComponent<Animator>();
        comboManager = FindObjectOfType<ComboManager>();
        CurrentScore = 0;
        txtScore.text = "0";
    }

    public void IncreaseScore(int p_JudgementState)
    {

        //콤보 증가
        comboManager.IncreaseCombo();

        //현재 콤보 return받아 가져옴
        int t_currentCombo = comboManager.GetCurrentCombo();
        //기본 점수 10점에 콤보구간 10~19 = 10점 20~29 = 20점 가중치로 추가 ...... 
        int t_bonusScore = (t_currentCombo / 10) * 10;

        //점수 증가
        int t_increaseScore = increaseScore + t_bonusScore;

        //가중치 판정따라 계산
        t_increaseScore = (int)(t_increaseScore * weight[p_JudgementState]);

        CurrentScore += t_increaseScore;
        //0은 항상 보여주고 천의 자리마다 쉼표 넣어주는 문자열 포맷
        txtScore.text = string.Format("{0:#,##0}", CurrentScore);

        anim.SetTrigger(ScoreUp);
    }

    public int GetCurrentScore()
    {
        return CurrentScore;
    }
}
