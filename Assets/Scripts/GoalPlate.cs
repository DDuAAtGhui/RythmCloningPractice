using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPlate : MonoBehaviour
{
    AudioSource Audio;
    NoteManager noteManager;
    ResultMenu resultMenu;
    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioSource>();
        noteManager = FindObjectOfType<NoteManager>();
        resultMenu = FindObjectOfType<ResultMenu>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Audio.Play();
            PlayerController.s_canPressKey = false;
            noteManager.RemoveNote();
            resultMenu.showResult();
        }
    }
}
