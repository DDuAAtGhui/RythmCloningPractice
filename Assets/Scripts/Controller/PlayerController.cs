using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move Info")]
    [SerializeField] float MoveSpeed = 3f;
    Vector3 dir = new Vector3();
    Vector3 destination = new Vector3();
    //그냥 미끌어지듯이 움직이면 어색하니까 회전 넣어줄것
    [SerializeField] float spinSpeed = 270f;
    Vector3 spinDir = new Vector3();
    Quaternion spinDestination = new Quaternion();

    //들썩거리는 반동 구현
    [SerializeField] float recoilPosY = 0.25f;
    [SerializeField] float recoilSpeed = 1.5f;

    //회전 값을 구하기 위해 가짜 큐브를 만들것임
    //가짜 큐브를 먼저 돌려놓고 그 돌아간 만큼의 값을 목표 회전값으로 삼을것
    [SerializeField] Transform fakeCube;
    //회전시킬 객체
    [SerializeField] Transform realCube;

    //구르는 동안에 키 입력 들어가면 급발진 하는 문제 있으니 그거 막아줌
    //canMove가 true면 움직이기 가능
    bool canMove = true;


    TimingManager timingManager;
    CameraController Cam;
    void Start()
    {
        timingManager = FindObjectOfType<TimingManager>();
        Cam = FindObjectOfType<CameraController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            if (canMove)
            {  //판정 박스 안에 노트가 있을때 키 입력한게 true일때만 움직이기
                if (timingManager.CheckTheTiming())
                {
                    StartAction();
                }
            }
        }
    }

    public void StartAction()
    {
        //방향 계산
        //X축(좌 우)이 위 아래 입력키
        //Z축(앞 뒤)이 좌 우 입력키 로 만들기로 정하고 갔기때문
        dir.Set(Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));

        //이동 목표값 계산
        //아래키 누를때 X+방향으로 진행하게 해야해서 방향이 반대임. 그래서 - 넣어준거
        destination = transform.position + new Vector3(-dir.x, 0, dir.z);

        //회전 목표값 계산. 인스펙터에서 회전 만져보면서 확인해보면 알아보기 쉬움
        //오른쪽 누르면 Z반대 방향으로 가니 회전도 그에 맞춰서
        //아래 누르면 X방향으로 가니 회전도 그에 맞춰서
        spinDir = new Vector3(-dir.z, 0f, -dir.x);

        //편법 회전 구현 - 자신의 공전 목표물을 자신으로, 축을 회전방향으로
        fakeCube.RotateAround(transform.position, spinDir, spinSpeed);
        //위 회전값을 spinDestination에 넣어줌
        spinDestination = fakeCube.rotation;

        StartCoroutine(MoveCo());
        StartCoroutine(SpinCo());
        StartCoroutine(RecoilCo());
        StartCoroutine(Cam.ZoomCam());
    }

    //움직임을 코루틴으로 구현
    IEnumerator MoveCo()
    {
        //움직임 행해지는동안 키 입력해도 추가로 안움직이게 bool값 false로
        canMove = false;

        //목적지까지 도달하기까지 반복
        //while(Vector3.Distance(transform.position, destination) != 0)
        //{

        //}

        //Distance보다 가벼운 버전
        while (Vector3.SqrMagnitude(transform.position - destination) >= 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, MoveSpeed * Time.deltaTime);

            //1프레임 휴게
            yield return null;
        }
        //0.001근처 오면 while탈출이니까 아주 근소한 소수점의 오차가 생김.
        //움직인 뒤 현재 위치 오차 제거 작업
        transform.position = destination;

        //움직임이 끝나면 다시 움직일 수 있게 bool값 true로 
        canMove = true;
    }

    IEnumerator SpinCo()
    {
        //움직임 행해지는동안 키 입력해도 추가로 안움직이게 bool값 false로
        canMove = false;

        //두 rotation 사이의 각도가 0.5도보다 클때만 실행
        while (Quaternion.Angle(realCube.rotation, spinDestination) > 0.5f)
        {
            realCube.rotation = Quaternion.RotateTowards(realCube.rotation, spinDestination, spinSpeed * Time.deltaTime);

            //1프레임 휴게
            yield return null;
        }
        //회전하고 난 뒤 현재 위치 각도 오차 제거
        realCube.rotation = spinDestination;
        //움직임이 끝나면 다시 움직일 수 있게 bool값 true로 
        canMove = true;

    }

    IEnumerator RecoilCo()
    {
        //움직임 행해지는동안 키 입력해도 추가로 안움직이게 bool값 false로
        canMove = false;

        //recoilPosY이 최대값
        while (realCube.position.y < recoilPosY)
        {
            //readCube의 position.y값을 시간계수 곱해서 점점 올려준다
            realCube.position += new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            //1프레임 휴게
            yield return null;

        }
        //반동 y축 최대값 됐을때 탈출

        //탈출 후 0될때까지 점차 내려준다
        while (realCube.position.y > 0)
        {
            //readCube의 position.y값을 시간계수 곱해서 점점 빼준다
            realCube.position -= new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            //1프레임 휴게
            yield return null;
        }

        //다시 내려오면 완전히 원위치
        realCube.localPosition = new Vector3(0, 0, 0);
        //움직임이 끝나면 다시 움직일 수 있게 bool값 true로 
        canMove = true;

    }
}
