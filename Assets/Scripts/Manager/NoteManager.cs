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

    //노트 추가시켜야하니 선언
    TimingManager timingManager;

    //Miss이펙트 사용해야함
    EffectManager effectManager;

    //화면 밖으로 벗어나서 Miss뜰때 콤보 초기화해야함
    ComboManager comboManager;

    bool noteActive = true;
    private void Start()
    {
        //타이밍 매니저 사용해야하니 스크립트 컴포넌트 형태로 가져옴
        timingManager = GetComponent<TimingManager>();
        effectManager = GetComponentInChildren<EffectManager>();
        comboManager = FindObjectOfType<ComboManager>();
    }
    void Update()
    {
        if (noteActive)
        {
            //60초 나누기 BPM = 1비트당 시간간격 초.
            //120BPM이면 0.5초의 간격을 가짐.
            TimeIntervalWithBeat = 60d / BPM;
            //BPM 나누기 1 분 = 1초당 비트
            BPS = BPM / 60d;
            currenTime += Time.deltaTime;

            if (currenTime >= TimeIntervalWithBeat)
            {
                //오브젝트 풀에 들있는 노트 큐에서 가장 먼저 들어온 객체를 꺼내옴
                GameObject t_note = ObjectPool.instance.noteQueue.Dequeue();
                //꺼내온 오브젝트 위치 설정
                t_note.transform.position = NoteAppearPosition.position;
                t_note.SetActive(true);

                //꺼내온 노트 리스트에 추가
                timingManager.boxNoteList.Add(t_note);

                //Time.deltatime에 소수점 오차가 존재하니 아예 깔끔하게 초기화
                currenTime -= TimeIntervalWithBeat;
            }
        }
    }

    //화면 밖으로 벗어난거 체크하는 콜라이더 부분
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Note"))
        {
            //노트가 스페이스바 눌러서 판정박스 안에서 체크 될때 반환 하는게 아니고
            //이미지만 꺼두고 화면 밖 콜라이더에서 만날때 반환되는 중이기 때문
            //이미지가 enabled 상태일때
            if (collision.GetComponent<Note>().GetNoteFlag())
            {   //화면 밖으로 벗어나면 Miss출력
                effectManager.JudgementEffect(4);
                comboManager.ResetCombo();
            }


            //노트 반납과 동시에 리스트에서 제거
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
