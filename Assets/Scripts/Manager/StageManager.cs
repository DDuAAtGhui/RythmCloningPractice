using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] GameObject stage;
    Transform[] stagePlates;

    //�÷���Ʈ �Ʒ����� ���� �ö󰡴� �Ÿ�
    [SerializeField] float OffSetY = 5f;
    [SerializeField] float OffsetY_Speed = 10f;

    //�÷��̾� ������
    int stepCount = 0;

    int totalPlates;
    void Start()
    {
        stagePlates = stage.GetComponent<Stage>().plates;
        totalPlates = stagePlates.Length;

        //���� �� �� Y������ �������� �ص�
        for (int i = 0; i < totalPlates; i++)
            stagePlates[i].position = new Vector3(stagePlates[i].position.x, stagePlates[i].position.y - OffSetY,
                stagePlates[i].position.z);
    }

    public void ShowNextPlate()
    {
        if (stepCount < totalPlates)
            StartCoroutine(MovePlate(stepCount++));

    }

    IEnumerator MovePlate(int num)
    {
        stagePlates[num].gameObject.SetActive(true);
        Vector3 destination_Pos = new Vector3(stagePlates[num].position.x,
            stagePlates[num].position.y + OffSetY, stagePlates[num].position.z);

        while (Vector3.SqrMagnitude(stagePlates[num].position - destination_Pos) >= 0.001f)
        {
            stagePlates[num].position = Vector3.Lerp(stagePlates[num].position, destination_Pos,
                OffsetY_Speed * Time.deltaTime);

            yield return null;
        }

        //0.000000���������� ����
        stagePlates[num].position = destination_Pos;
    }
}
