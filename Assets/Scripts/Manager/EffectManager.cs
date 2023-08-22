using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    [SerializeField] Animator noteHitAnimator;

    #region 저지먼트이펙트
    [SerializeField] Animator judgementAnimator;

    //교체할 이미지 변수
    [SerializeField] Image judgementIMG;

    //Perfect~Miss 담아둘 스프라이트 배열. 0 1 2 3 4 순으로 담아주기
    [SerializeField] Sprite[] judgementSprites;
    #endregion

    //애니메이터에서 선언한 Hit트리거 파라미터의 이름을 담고있는 변수
    string Hit = "Hit";

    //파라미터 값에 맞는 이미지 스프라이트로 교체할것
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
