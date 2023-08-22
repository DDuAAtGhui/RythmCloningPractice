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
        //���۽ÿ� �Ⱥ���
        goComboImage.SetActive(false);
        ComboText.gameObject.SetActive(false);
        ComboTextImage = GetComponentInChildren<Image>();
        anim = GetComponent<Animator>();
    }
    //�Ķ���Ϳ� �ƹ� �Էµ� ���ϸ� 1�� �⺻������ �ϰڴٴ� ��
    public void IncreaseCombo(int p_num = 1)
    {
        CurrentCombo += p_num;
        ComboText.text = string.Format("{0:#,##0}", CurrentCombo);

        //2�� ���� �������������� ���̰�
        if (CurrentCombo > 2)
        {
            goComboImage.SetActive(true);
            ComboText.gameObject.SetActive(true);

            anim.SetTrigger(ComboUp);
        }
    }
    //�ܺο��� �޺� �� ��� �� �� �ְ� ��ȯ
    public int GetCurrentCombo()
    {
        return CurrentCombo;
    }
    //�޺� �ʱ�ȭ
    public void ResetCombo()
    {
        CurrentCombo = 0;
        ComboText.text = "0";
        goComboImage.SetActive(false);
        ComboText.gameObject.SetActive(false);

    }
}
