using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    public float NoteSpeed = 400f;

    //판정 체크할때 Destroy 대신 이미지 끄기 기능 사용할거임
    //이유 : 중간 바로 다음에서 노트 콜라이더 충돌하면 오디오 나오게 했는데 디스트로이하면 영원히 안나올수있으니깐
    Image NoteImage;

    private void OnEnable()
    {
        //Getcomponent 쓸데없이 무한으로 안가져오게
        if(NoteImage == null)
             NoteImage = GetComponent<Image>();

        //타이밍 매니저쪽에서 판정박스 안에서 이미지 enable false로 설정했으니까
        //노트 오브젝트 풀에서 빌려올때 SetActive true되니까 이미지도 같이 켜줌
        NoteImage.enabled = true;
    }
    void Update()
    {
        //UI이미지가 캔버스 내에서 움직이는 거니까 local좌표로
        transform.position += Vector3.right * NoteSpeed * Time.deltaTime;
    }

    public void HideImage()
    {
        NoteImage.enabled = false;
    }
    
    //외부에서 노트 이미지가 활성화 되어있는게 참인지 거짓인지 확인해야함
    //오디오 나오게 하는 용도로 이미지만 꺼둬서 미스가 무한으로 뜨는 문제가 있기때문에
    public bool GetNoteFlag()
    {
        return NoteImage.enabled;
    }
}
