using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    [SerializeField] Animator noteHitAnimator;

    #region ������Ʈ����Ʈ
    [SerializeField] Animator judgementAnimator;

    //��ü�� �̹��� ����
    [SerializeField] Image judgementIMG;

    //Perfect~Miss ��Ƶ� ��������Ʈ �迭. 0 1 2 3 4 ������ ����ֱ�
    [SerializeField] Sprite[] judgementSprites;
    #endregion

    //�ִϸ����Ϳ��� ������ HitƮ���� �Ķ������ �̸��� ����ִ� ����
    string Hit = "Hit";

    //�Ķ���� ���� �´� �̹��� ��������Ʈ�� ��ü�Ұ�
    public void JudgementEffect(int p_num)
    {
        judgementIMG.sprite = judgementSprites[p_num];
        judgementAnimator.SetTrigger(Hit);
    }

    public void NoteHitEffect()
    {
        noteHitAnimator.SetTrigger(Hit);
    }
}
