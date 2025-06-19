using UnityEngine;
using UnityEngine.UI;

//������ ����������� ���������� ��������
public class DrowCounter : MonoBehaviour
{
    [SerializeField] private Text _text;    //��������� ������

    //����� ���������
    public void DrowCountResurs(int count)
    { 
        _text.text = count.ToString();  //������������ ���������� �������� � ����
    }
}
