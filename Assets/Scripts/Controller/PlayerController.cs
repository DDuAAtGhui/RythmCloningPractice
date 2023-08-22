using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move Info")]
    [SerializeField] float MoveSpeed = 3f;
    Vector3 dir = new Vector3();
    Vector3 destination = new Vector3();
    //�׳� �̲��������� �����̸� ����ϴϱ� ȸ�� �־��ٰ�
    [SerializeField] float spinSpeed = 270f;
    Vector3 spinDir = new Vector3();
    Quaternion spinDestination = new Quaternion();

    //���Ÿ��� �ݵ� ����
    [SerializeField] float recoilPosY = 0.25f;
    [SerializeField] float recoilSpeed = 1.5f;

    //ȸ�� ���� ���ϱ� ���� ��¥ ť�긦 �������
    //��¥ ť�긦 ���� �������� �� ���ư� ��ŭ�� ���� ��ǥ ȸ�������� ������
    [SerializeField] Transform fakeCube;
    //ȸ����ų ��ü
    [SerializeField] Transform realCube;

    //������ ���ȿ� Ű �Է� ���� �޹��� �ϴ� ���� ������ �װ� ������
    //canMove�� true�� �����̱� ����
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
            {  //���� �ڽ� �ȿ� ��Ʈ�� ������ Ű �Է��Ѱ� true�϶��� �����̱�
                if (timingManager.CheckTheTiming())
                {
                    StartAction();
                }
            }
        }
    }

    public void StartAction()
    {
        //���� ���
        //X��(�� ��)�� �� �Ʒ� �Է�Ű
        //Z��(�� ��)�� �� �� �Է�Ű �� ������ ���ϰ� ���⶧��
        dir.Set(Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));

        //�̵� ��ǥ�� ���
        //�Ʒ�Ű ������ X+�������� �����ϰ� �ؾ��ؼ� ������ �ݴ���. �׷��� - �־��ذ�
        destination = transform.position + new Vector3(-dir.x, 0, dir.z);

        //ȸ�� ��ǥ�� ���. �ν����Ϳ��� ȸ�� �������鼭 Ȯ���غ��� �˾ƺ��� ����
        //������ ������ Z�ݴ� �������� ���� ȸ���� �׿� ���缭
        //�Ʒ� ������ X�������� ���� ȸ���� �׿� ���缭
        spinDir = new Vector3(-dir.z, 0f, -dir.x);

        //��� ȸ�� ���� - �ڽ��� ���� ��ǥ���� �ڽ�����, ���� ȸ����������
        fakeCube.RotateAround(transform.position, spinDir, spinSpeed);
        //�� ȸ������ spinDestination�� �־���
        spinDestination = fakeCube.rotation;

        StartCoroutine(MoveCo());
        StartCoroutine(SpinCo());
        StartCoroutine(RecoilCo());
        StartCoroutine(Cam.ZoomCam());
    }

    //�������� �ڷ�ƾ���� ����
    IEnumerator MoveCo()
    {
        //������ �������µ��� Ű �Է��ص� �߰��� �ȿ����̰� bool�� false��
        canMove = false;

        //���������� �����ϱ���� �ݺ�
        //while(Vector3.Distance(transform.position, destination) != 0)
        //{

        //}

        //Distance���� ������ ����
        while (Vector3.SqrMagnitude(transform.position - destination) >= 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, MoveSpeed * Time.deltaTime);

            //1������ �ް�
            yield return null;
        }
        //0.001��ó ���� whileŻ���̴ϱ� ���� �ټ��� �Ҽ����� ������ ����.
        //������ �� ���� ��ġ ���� ���� �۾�
        transform.position = destination;

        //�������� ������ �ٽ� ������ �� �ְ� bool�� true�� 
        canMove = true;
    }

    IEnumerator SpinCo()
    {
        //������ �������µ��� Ű �Է��ص� �߰��� �ȿ����̰� bool�� false��
        canMove = false;

        //�� rotation ������ ������ 0.5������ Ŭ���� ����
        while (Quaternion.Angle(realCube.rotation, spinDestination) > 0.5f)
        {
            realCube.rotation = Quaternion.RotateTowards(realCube.rotation, spinDestination, spinSpeed * Time.deltaTime);

            //1������ �ް�
            yield return null;
        }
        //ȸ���ϰ� �� �� ���� ��ġ ���� ���� ����
        realCube.rotation = spinDestination;
        //�������� ������ �ٽ� ������ �� �ְ� bool�� true�� 
        canMove = true;

    }

    IEnumerator RecoilCo()
    {
        //������ �������µ��� Ű �Է��ص� �߰��� �ȿ����̰� bool�� false��
        canMove = false;

        //recoilPosY�� �ִ밪
        while (realCube.position.y < recoilPosY)
        {
            //readCube�� position.y���� �ð���� ���ؼ� ���� �÷��ش�
            realCube.position += new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            //1������ �ް�
            yield return null;

        }
        //�ݵ� y�� �ִ밪 ������ Ż��

        //Ż�� �� 0�ɶ����� ���� �����ش�
        while (realCube.position.y > 0)
        {
            //readCube�� position.y���� �ð���� ���ؼ� ���� ���ش�
            realCube.position -= new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            //1������ �ް�
            yield return null;
        }

        //�ٽ� �������� ������ ����ġ
        realCube.localPosition = new Vector3(0, 0, 0);
        //�������� ������ �ٽ� ������ �� �ְ� bool�� true�� 
        canMove = true;

    }
}
