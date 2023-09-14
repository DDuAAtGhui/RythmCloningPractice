using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{

    //생성된 노트들을 담을 리스트. 판정 박스 안에 리스트 안 노트들이 들어있는지 확인해야함
    public List<GameObject> boxNoteList = new List<GameObject>();

    //판정들을 기록할 배열변수
    //결과창에서 Normal 안띄울꺼니까 Normal은 기록 안할거임
    int[] judgeRecord = new int[5];

    //판정의 중심지점
    [SerializeField] Transform Center;

    //Perfect Cool Good Bad의 판정박스들
    [SerializeField] RectTransform[] TimingBoxs;

    //판정 범위의 최소값 x, 최대값 y로 사용할것
    Vector2[] TimingBoxsWidth;

    //히트박스 체크 디버그용 딕셔너리
    Dictionary<int, string> HitBoxDebugDic = new Dictionary<int, string>
    {
        {0,"Perfect" },
        {1,"Cool" },
        {2,"Good" },
        {3,"Bad" },
    };
    //노트 판정 맞을때 이펙트 터뜨려야하니 이펙트매니저 참조
    EffectManager effectManager;

    //점수도 추가시켜야하니 스코어매니저 참조
    ScoreManager scoreManager;

    //노트 떨궜을때 콤보 초기화 시켜야하니 콤보매니저 참조
    ComboManager comboManager;

    StageManager stageMager;
    PlayerController playerController;
    void Start()
    {
        //EffectManager 스크립트 달린 오브젝트가 자식인 상태임
        effectManager = GetComponentInChildren<EffectManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        comboManager = FindObjectOfType<ComboManager>();
        stageMager = FindObjectOfType<StageManager>();
        playerController = FindObjectOfType<PlayerController>();
        //배열 길이 동일화
        //4개의 판정박스들만큼 4개의 벡터2 X,Y가 생성됨
        TimingBoxsWidth = new Vector2[TimingBoxs.Length];

        #region 판정박스 순서
        // i 0 = Perfect
        // i 1 = Cool
        // i 2 = Good
        // i 3 = Bad의
        #endregion
        for (int i = 0; i < TimingBoxs.Length; i++)
        {
            //각각 판정박스의 Vector2값을 세팅해줌
            //X를 판정 최소값 Y를 판정 최대값으로 사용할것이므로
            //최소값인 X를 중심.x - i번 판정박스의 이미지 너비 / 2 로 설정 (중심에서 절반만큼 왼쪽으로 간 포인트 )
            //최대값인 Y를 중심.x + i번 판정박스의 이미지 너비 / 2 로 설정 (중심에서 절반만큼 오른쪽으로 간 포인트 )
            //이러면 양 너비로 이루어진 판정박스 i번째 Vector2값을 Set으로 설정함
            TimingBoxsWidth[i].Set(Center.localPosition.x - TimingBoxs[i].rect.width / 2,
                                   Center.localPosition.x + TimingBoxs[i].rect.width / 2);


        }

    }

    //리스트 안에 있는 노트들이 판정 박스 범위 안에 있는지 체크
    public bool CheckTheTiming()
    {
        for (int i = 0; i < boxNoteList.Count; i++)
        {
            //이 값이 위 Vector2 X,Y 사이 안에 들어왔는지 체크할거임
            float Timing_notePos_X = boxNoteList[i].transform.localPosition.x;

            for (int x = 0; x < TimingBoxsWidth.Length; x++)
            {
                //x번째 판정박스의 Vector position x값(최소값)이 노트보다 작고 = 최소값보단 노트가 오른쪽에 있고
                //x번째 판정박스의 Vector position y값(최대값)이 노트보다 크면 = 최대값보다 노트가 오른쪽에 있다면
                if (TimingBoxsWidth[x].x <= Timing_notePos_X && TimingBoxsWidth[x].y >= Timing_notePos_X)
                {
                    //판정 들어갔으면 이미지 숨김
                    boxNoteList[i].GetComponent<Note>().HideImage();
                    //i번째 제거
                    boxNoteList.RemoveAt(i);

                    //노트 이펙트 호출
                    //0~2인덱스만 이펙트 호출. 인덱스 3번은 Bad니까 이펙트 뜨면 이상하니까
                    if (x < TimingBoxsWidth.Length - 1)
                    {
                        effectManager.NoteHitEffect();

                        if (CheckCanNextPlate())
                        {
                            //점수 증가. BAD때는 점수 증가 없게
                            scoreManager.IncreaseScore(x);
                            stageMager.ShowNextPlate();

                            //Index 0부터 체크하므로 체크 순서도 Perfect ~~~ Bad 순서임
                            effectManager.JudgementEffect(x); //판정연출

                            judgeRecord[x]++; //판정기록
                        }
                        else
                            effectManager.JudgementEffect(5);

                    }

                    //X인덱스가 BAD이면
                    else if (x == TimingBoxsWidth.Length - 1) comboManager.ResetCombo();

                    //체크 끝났으면 의미없는 반복 할 필요 없으니 바로 종료하고 true 반환
                    return true;
                }
            }
        }
        comboManager.ResetCombo();
        //검색 실패시 Miss 출력
        effectManager.JudgementEffect(4);
        MissRecord(); //Miss 판정기록

        //Miss시 false 반환
        return false;
    }

    bool CheckCanNextPlate()
    {
        if (Physics.Raycast(playerController.destination, Vector3.down, out RaycastHit hitInfo, 1.1f))
        {
            if (hitInfo.transform.CompareTag("BasicPlate"))
            {
                BasicPlate plate = hitInfo.transform.GetComponent<BasicPlate>();
                if (plate.flag)
                {
                    plate.flag = false;
                    return true;
                }
            }
        }

        return false;
    }

    public int[] GetJudgeRecord()
    {
        return judgeRecord;
    }

    public void MissRecord() => judgeRecord[4]++; //Miss 판정기록

}
