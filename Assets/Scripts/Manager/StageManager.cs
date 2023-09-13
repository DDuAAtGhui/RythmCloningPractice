using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] GameObject stage;
    Transform[] stagePlates;

    //플레이트 아래에서 위로 올라가는 거리
    [SerializeField] float OffSetY = 5f;
    [SerializeField] float OffsetY_Speed = 10f;

    //플레이어 움직임
    int stepCount = 0;

    int totalPlates;
    void Start()
    {
        stagePlates = stage.GetComponent<Stage>().plates;
        totalPlates = stagePlates.Length;

        //시작 할 때 Y축으로 내려가게 해둠
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

        //0.000000오차같은거 제거
        stagePlates[num].position = destination_Pos;
    }
}
