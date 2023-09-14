using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    #region ���ھ�
    [SerializeField] Text txtScore;
    [SerializeField] int increaseScore = 10;

    int CurrentScore = 0;

    //���� ���� ���� ����ġ. 
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

        //�޺� ����
        comboManager.IncreaseCombo();

        //���� �޺� return�޾� ������
        int t_currentCombo = comboManager.GetCurrentCombo();
        //�⺻ ���� 10���� �޺����� 10~19 = 10�� 20~29 = 20�� ����ġ�� �߰� ...... 
        int t_bonusScore = (t_currentCombo / 10) * 10;

        //���� ����
        int t_increaseScore = increaseScore + t_bonusScore;

        //����ġ �������� ���
        t_increaseScore = (int)(t_increaseScore * weight[p_JudgementState]);

        CurrentScore += t_increaseScore;
        //0�� �׻� �����ְ� õ�� �ڸ����� ��ǥ �־��ִ� ���ڿ� ����
        txtScore.text = string.Format("{0:#,##0}", CurrentScore);

        anim.SetTrigger(ScoreUp);
    }

    public int GetCurrentScore()
    {
        return CurrentScore;
    }
}
