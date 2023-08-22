using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    [SerializeField] GameObject goComboImage;
    [SerializeField] Text ComboText;

    int CurrentCombo = 0;

    Animator anim;
    Image ComboTextImage;

    string ComboUp = "ComboUp";
    private void Start()
    {
        //시작시엔 안보임
        goComboImage.SetActive(false);
        ComboText.gameObject.SetActive(false);
        ComboTextImage = GetComponentInChildren<Image>();
        anim = GetComponent<Animator>();
    }
    //파라미터에 아무 입력도 안하면 1을 기본값으로 하겠다는 뜻
    public void IncreaseCombo(int p_num = 1)
    {
        CurrentCombo += p_num;
        ComboText.text = string.Format("{0:#,##0}", CurrentCombo);

        //2번 연속 성공했을때부터 보이게
        if (CurrentCombo > 2)
        {
            goComboImage.SetActive(true);
            ComboText.gameObject.SetActive(true);

            anim.SetTrigger(ComboUp);
        }
    }
    //외부에서 콤보 값 사용 할 수 있게 반환
    public int GetCurrentCombo()
    {
        return CurrentCombo;
    }
    //콤보 초기화
    public void ResetCombo()
    {
        CurrentCombo = 0;
        ComboText.text = "0";
        goComboImage.SetActive(false);
        ComboText.gameObject.SetActive(false);

    }
}
