using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    public float NoteSpeed = 400f;

    //���� üũ�Ҷ� Destroy ��� �̹��� ���� ��� ����Ұ���
    //���� : �߰� �ٷ� �������� ��Ʈ �ݶ��̴� �浹�ϸ� ����� ������ �ߴµ� ��Ʈ�����ϸ� ������ �ȳ��ü������ϱ�
    Image NoteImage;

    private void OnEnable()
    {
        //Getcomponent �������� �������� �Ȱ�������
        if(NoteImage == null)
             NoteImage = GetComponent<Image>();

        //Ÿ�̹� �Ŵ����ʿ��� �����ڽ� �ȿ��� �̹��� enable false�� ���������ϱ�
        //��Ʈ ������Ʈ Ǯ���� �����ö� SetActive true�Ǵϱ� �̹����� ���� ����
        NoteImage.enabled = true;
    }
    void Update()
    {
        //UI�̹����� ĵ���� ������ �����̴� �Ŵϱ� local��ǥ��
        transform.position += Vector3.right * NoteSpeed * Time.deltaTime;
    }

    public void HideImage()
    {
        NoteImage.enabled = false;
    }
    
    //�ܺο��� ��Ʈ �̹����� Ȱ��ȭ �Ǿ��ִ°� ������ �������� Ȯ���ؾ���
    //����� ������ �ϴ� �뵵�� �̹����� ���ּ� �̽��� �������� �ߴ� ������ �ֱ⶧����
    public bool GetNoteFlag()
    {
        return NoteImage.enabled;
    }
}
