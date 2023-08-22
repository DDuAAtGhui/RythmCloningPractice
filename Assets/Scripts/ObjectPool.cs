using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//��ü�� ������ ����ִ� Ŭ���� ����
//�ν����� â���� �̸� ������ ��ü �־�����ϴ� Ŭ���� ����ȭ
[System.Serializable]
public class ObjectInfo
{
    public GameObject goPrefab;
    public int count;
    //TFPoolParent �Ʒ��� �ڽĿ��� �����ǰ� �Ұ��� (Canvas�ϱ�)
    public Transform TFPoolParent;
}
public class ObjectPool : MonoBehaviour
{
    //�迭�� ObjectInfoŬ���� �ҷ���
    [SerializeField] ObjectInfo[] ObejctInfos;

    //�����ڿ� �ν��Ͻ��� ���� ��𼭵� �ְ� ������ �����Ӱ�
    public static ObjectPool instance;

    //�����Ͱ� ���Լ��� �������
    public Queue<GameObject> noteQueue = new Queue<GameObject>();

    private void Start()
    {
        //�ڱ� �ڽ� ���� �־��༭ ������ �Ҵ�
        instance = this;
        //��Ʈ ť�� �迭 ������Ʈ���� 0��° ��ü �־���
        noteQueue = InsterQueue(ObejctInfos[0]);

    }

    //GameObject������ ť�� ���Ͻ�Ű�� �Լ�
    Queue<GameObject> InsterQueue(ObjectInfo p_objectInfo)
    {
        //�ӽ� ť
        Queue<GameObject> temp_queue = new Queue<GameObject>();

        for(int i = 0; i<p_objectInfo.count; i++)
        {
            GameObject t_clone = Instantiate(p_objectInfo.goPrefab, transform.position, Quaternion.identity);
            t_clone.SetActive(false);

            //TFPoolParent�� �θ� ����ߴٸ�
            if (p_objectInfo.TFPoolParent != null)
                //�θ�� �����
                t_clone.transform.SetParent(p_objectInfo.TFPoolParent); 

            //�θ� ��� �������� �� ��ũ��Ʈ �پ��ִ� ������Ʈ�� �θ�� �����
            else
                t_clone.transform.SetParent(this.transform);

            //ť�� p_objectInfo.count ������ŭ �־���.
            temp_queue.Enqueue(t_clone);
        }

        return temp_queue;
    }
}