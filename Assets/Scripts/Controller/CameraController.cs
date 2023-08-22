using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Main Camera에 어태치
public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target_Player;
    [SerializeField] float followSpeed = 15f;

    Vector3 playerDistance = new Vector3();

    float hitDistance = 0f;
    //줌 아웃
    [SerializeField] float zoomDistance = -1.25f;
    void Start()
    {
        playerDistance = transform.position - target_Player.position;
    }


    void Update()
    {
        //hitDistance가 0일땐 카메라 목적지 포지션엔 변화가 없지만 hitDistance가 변하면 
        //카메라의 forward(Z축) 방향으로 포지션이 이동하게됨
        Vector3 t_destPos = target_Player.position + playerDistance + (transform.forward * hitDistance);

        //followSpeed * Time.deltaTime 값으로 보간
        transform.position = Vector3.Lerp(transform.position, t_destPos, followSpeed * Time.deltaTime);
    }

    //PlayerController에서 호출함
    public IEnumerator ZoomCam()
    {
        hitDistance = zoomDistance;

        yield return new WaitForSeconds(0.15f);

        hitDistance = 0f;
    }
}
