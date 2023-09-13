using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int BPM = 0;
    public double BPS = 0;
    public double TimeIntervalWithBeat = 0;
    double currenTime = 0d;
    [SerializeField] Transform NoteAppearPosition;

    //��Ʈ �߰����Ѿ��ϴ� ����
    TimingManager timingManager;

    //Miss����Ʈ ����ؾ���
    EffectManager effectManager;

    //ȭ�� ������ ����� Miss�㶧 �޺� �ʱ�ȭ�ؾ���
    ComboManager comboManager;

    bool noteActive = true;
    private void Start()
    {
        //Ÿ�̹� �Ŵ��� ����ؾ��ϴ� ��ũ��Ʈ ������Ʈ ���·� ������
        timingManager = GetComponent<TimingManager>();
        effectManager = GetComponentInChildren<EffectManager>();
        comboManager = FindObjectOfType<ComboManager>();
    }
    void Update()
    {
        if (noteActive)
        {
            //60�� ������ BPM = 1��Ʈ�� �ð����� ��.
            //120BPM�̸� 0.5���� ������ ����.
            TimeIntervalWithBeat = 60d / BPM;
            //BPM ������ 1 �� = 1�ʴ� ��Ʈ
            BPS = BPM / 60d;
            currenTime += Time.deltaTime;

            if (currenTime >= TimeIntervalWithBeat)
            {
                //������Ʈ Ǯ�� ���ִ� ��Ʈ ť���� ���� ���� ���� ��ü�� ������
                GameObject t_note = ObjectPool.instance.noteQueue.Dequeue();
                //������ ������Ʈ ��ġ ����
                t_note.transform.position = NoteAppearPosition.position;
                t_note.SetActive(true);

                //������ ��Ʈ ����Ʈ�� �߰�
                timingManager.boxNoteList.Add(t_note);

                //Time.deltatime�� �Ҽ��� ������ �����ϴ� �ƿ� ����ϰ� �ʱ�ȭ
                currenTime -= TimeIntervalWithBeat;
            }
        }
    }

    //ȭ�� ������ ����� üũ�ϴ� �ݶ��̴� �κ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Note"))
        {
            //��Ʈ�� �����̽��� ������ �����ڽ� �ȿ��� üũ �ɶ� ��ȯ �ϴ°� �ƴϰ�
            //�̹����� ���ΰ� ȭ�� �� �ݶ��̴����� ������ ��ȯ�Ǵ� ���̱� ����
            //�̹����� enabled �����϶�
            if (collision.GetComponent<Note>().GetNoteFlag())
            {   //ȭ�� ������ ����� Miss���
                effectManager.JudgementEffect(4);
                comboManager.ResetCombo();
            }


            //��Ʈ �ݳ��� ���ÿ� ����Ʈ���� ����
            timingManager.boxNoteList.Remove(collision.gameObject);
            ObjectPool.instance.noteQueue.Enqueue(collision.gameObject);
            collision.gameObject.SetActive(false);

        }
    }

    public void RemoveNote()
    {
        noteActive = false;

        for (int i = 0; i < timingManager.boxNoteList.Count; i++)
        {
            timingManager.boxNoteList[i].SetActive(false);
            ObjectPool.instance.noteQueue.Enqueue(timingManager.boxNoteList[i]);
        }
    }
}
