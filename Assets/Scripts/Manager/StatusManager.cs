using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    bool isDead = false;

    int maxHP = 3;
    int currentHP;

    int maxShield = 3;
    int currentShield = 0;

    [SerializeField] GameObject[] HpImages;
    [SerializeField] GameObject[] ShieldImages;

    ResultMenu result;
    NoteManager noteManager;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        result = FindObjectOfType<ResultMenu>();
        noteManager = FindObjectOfType<NoteManager>();
    }

    public void DecreaseHP(int num = 1)
    {
        currentHP -= num;

        if (currentHP < 0)
        {
            isDead = true;
            result.showResult();
        }
        SettingHPImage();
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
}
