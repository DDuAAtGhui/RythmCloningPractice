using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Main Camera�� ����ġ
public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target_Player;
    [SerializeField] float followSpeed = 15f;

    Vector3 playerDistance = new Vector3();

    float hitDistance = 0f;
    //�� �ƿ�
    [SerializeField] float zoomDistance = -1.25f;
    void Start()
    {
        playerDistance = transform.position - target_Player.position;
    }


    void Update()
    {
        //hitDistance�� 0�϶� ī�޶� ������ �����ǿ� ��ȭ�� ������ hitDistance�� ���ϸ� 
        //ī�޶��� forward(Z��) �������� �������� �̵��ϰԵ�
        Vector3 t_destPos = target_Player.position + playerDistance + (transform.forward * hitDistance);

        //followSpeed * Time.deltaTime ������ ����
        transform.position = Vector3.Lerp(transform.position, t_destPos, followSpeed * Time.deltaTime);
    }

    //PlayerController���� ȣ����
    public IEnumerator ZoomCam()
    {
        hitDistance = zoomDistance;

        yield return new WaitForSeconds(0.15f);

        hitDistance = 0f;
    }
}
