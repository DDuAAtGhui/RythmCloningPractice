using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    //체력 닳때 플레이어 깜빡이게 할거임
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] float blinkSpeed = 0.1f;
    [SerializeField] int blinkCount = 10;
    int currentblinkCount = 0;
    bool isBlink = false;

    bool isDead = false;

    int maxHP = 3;
    int currentHP;

    int maxShield = 3;
    int currentShield = 0;

    [SerializeField] GameObject[] HpImages;
    [SerializeField] GameObject[] ShieldImages;

    [SerializeField] int StandardComboCountToGetSehild = 5;
    int currentShieldCombo = 0;
    [SerializeField] Image shieldGauge;

    ResultMenu result;
    NoteManager noteManager;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        result = FindObjectOfType<ResultMenu>();
        noteManager = FindObjectOfType<NoteManager>();
    }

    public void IncreaseHP(int num = 1)
    {
        currentHP += num;

        if (currentHP >= maxHP)
            currentHP = maxHP;

        SettingHPImage();
    }
    public void DecreaseHP(int num = 1)
    {
        if (!isBlink)
        {

            if (currentShield > 0)
                DecreaseShield();

            else
            {
                currentHP -= num;

                if (currentHP < 0)
                {
                    isDead = true;
                    result.showResult();
                    noteManager.RemoveNote();
                }
                else
                    StartCoroutine(BlinkCo());

                SettingHPImage();
            }
        }
    }

    void SettingHPImage()
    {
        for (int i = 0; i < HpImages.Length; i++)
        {
            if (i < currentHP)
                HpImages[i].SetActive(true);
            else
                HpImages[i].SetActive(false);
        }
    }

    public bool Dead()
    {
        return isDead;
    }

    public void CheckShield()
    {
        currentShieldCombo++;

        if (currentShieldCombo >= StandardComboCountToGetSehild)
        {
            IncreaseShield();
            currentShieldCombo = 0;
        }

        shieldGauge.fillAmount = (float)currentShieldCombo / StandardComboCountToGetSehild;
    }
    public void ResetShieldCombo()
    {
        currentShieldCombo = 0;
        shieldGauge.fillAmount = (float)currentShieldCombo / StandardComboCountToGetSehild;
    }
    public void IncreaseShield()
    {
        currentShield++;

        if (currentShield >= maxShield)
            currentShield = maxShield;

        SettingShieldImage();
    }
    public void DecreaseShield(int num = 1)
    {
        currentShield -= num;

        if (currentShield <= 0)
            currentShield = 0;

        SettingShieldImage();
    }

    void SettingShieldImage()
    {
        for (int i = 0; i < ShieldImages.Length; i++)
        {
            if (i < currentShield)
                ShieldImages[i].SetActive(true);
            else
                ShieldImages[i].SetActive(false);
        }
    }


    //체력 닳을때 깜박이게 할용도
    IEnumerator BlinkCo()
    {
        isBlink = true;
        while (currentblinkCount < blinkCount)
        {
            meshRenderer.enabled = !meshRenderer.enabled;
            yield return new WaitForSeconds(blinkSpeed);
            currentblinkCount++;
        }
        meshRenderer.enabled = true;
        isBlink = false;
        currentblinkCount = 0;
    }
}
