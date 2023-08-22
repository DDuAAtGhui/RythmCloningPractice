using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{

    //������ ��Ʈ���� ���� ����Ʈ. ���� �ڽ� �ȿ� ����Ʈ �� ��Ʈ���� ����ִ��� Ȯ���ؾ���
    public List<GameObject> boxNoteList = new List<GameObject>();

    //������ �߽�����
    [SerializeField] Transform Center;

    //Perfect Cool Good Bad�� �����ڽ���
    [SerializeField] RectTransform[] TimingBoxs;

    //���� ������ �ּҰ� x, �ִ밪 y�� ����Ұ�
    Vector2[] TimingBoxsWidth;

    //��Ʈ�ڽ� üũ ����׿� ��ųʸ�
    Dictionary<int, string> HitBoxDebugDic = new Dictionary<int, string>
    {
        {0,"Perfect" },
        {1,"Cool" },
        {2,"Good" },
        {3,"Bad" },
    };
    //��Ʈ ���� ������ ����Ʈ �Ͷ߷����ϴ� ����Ʈ�Ŵ��� ����
    EffectManager effectManager;

    //������ �߰����Ѿ��ϴ� ���ھ�Ŵ��� ����
    ScoreManager scoreManager;

    //��Ʈ �������� �޺� �ʱ�ȭ ���Ѿ��ϴ� �޺��Ŵ��� ����
    ComboManager comboManager;
    void Start()
    {
        //EffectManager ��ũ��Ʈ �޸� ������Ʈ�� �ڽ��� ������
        effectManager = GetComponentInChildren<EffectManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        comboManager = FindObjectOfType<ComboManager>();

        //�迭 ���� ����ȭ
        //4���� �����ڽ��鸸ŭ 4���� ����2 X,Y�� ������
        TimingBoxsWidth = new Vector2[TimingBoxs.Length];

        #region �����ڽ� ����
        // i 0 = Perfect
        // i 1 = Cool
        // i 2 = Good
        // i 3 = Bad��
        #endregion
        for (int i = 0; i < TimingBoxs.Length; i++)
        {
            //���� �����ڽ��� Vector2���� ��������
            //X�� ���� �ּҰ� Y�� ���� �ִ밪���� ����Ұ��̹Ƿ�
            //�ּҰ��� X�� �߽�.x - i�� �����ڽ��� �̹��� �ʺ� / 2 �� ���� (�߽ɿ��� ���ݸ�ŭ �������� �� ����Ʈ )
            //�ִ밪�� Y�� �߽�.x + i�� �����ڽ��� �̹��� �ʺ� / 2 �� ���� (�߽ɿ��� ���ݸ�ŭ ���������� �� ����Ʈ )
            //�̷��� �� �ʺ�� �̷���� �����ڽ� i��° Vector2���� Set���� ������
            TimingBoxsWidth[i].Set(Center.localPosition.x - TimingBoxs[i].rect.width / 2,
                                   Center.localPosition.x + TimingBoxs[i].rect.width / 2);


        }

    }

    //����Ʈ �ȿ� �ִ� ��Ʈ���� ���� �ڽ� ���� �ȿ� �ִ��� üũ
    public bool CheckTheTiming()
    {
        for (int i = 0; i < boxNoteList.Count; i++)
        {
            //�� ���� �� Vector2 X,Y ���� �ȿ� ���Դ��� üũ�Ұ���
            float Timing_notePos_X = boxNoteList[i].transform.localPosition.x;

            for (int x = 0; x < TimingBoxsWidth.Length; x++)
            {
                //x��° �����ڽ��� Vector position x��(�ּҰ�)�� ��Ʈ���� �۰� = �ּҰ����� ��Ʈ�� �����ʿ� �ְ�
                //x��° �����ڽ��� Vector position y��(�ִ밪)�� ��Ʈ���� ũ�� = �ִ밪���� ��Ʈ�� �����ʿ� �ִٸ�
                if (TimingBoxsWidth[x].x <= Timing_notePos_X && TimingBoxsWidth[x].y >= Timing_notePos_X)
                {
                    //���� ������ �̹��� ����
                    boxNoteList[i].GetComponent<Note>().HideImage();
                    //i��° ����
                    boxNoteList.RemoveAt(i);

                    //Index 0���� üũ�ϹǷ� üũ ������ Perfect ~~~ Bad ������
                    effectManager.JudgementEffect(x);

                    //��Ʈ ����Ʈ ȣ��
                    //0~2�ε����� ����Ʈ ȣ��. �ε��� 3���� Bad�ϱ� ����Ʈ �߸� �̻��ϴϱ�
                    if (x < TimingBoxsWidth.Length - 1)
                    {
                        effectManager.NoteHitEffect();
                        //���� ����. BAD���� ���� ���� ����
                        scoreManager.IncreaseScore(x);
                    }

                    //X�ε����� BAD�̸�
                    else if (x == TimingBoxsWidth.Length-1) comboManager.ResetCombo();


                    //üũ �������� �ǹ̾��� �ݺ� �� �ʿ� ������ �ٷ� �����ϰ� true ��ȯ
                    return true;
                }
            }
        }
        comboManager.ResetCombo();
        //�˻� ���н� Miss ���
        effectManager.JudgementEffect(4);

        //Miss�� false ��ȯ
        return false;
    }
}