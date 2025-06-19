using System;
using UnityEngine;

//������ �������� �������� � ����
public class ResursCounter : MonoBehaviour
{
    [SerializeField] DrowCounter _drowCounter;  //������ ��������� ��������

    private int _resursCounter = 0; //���������� ��������

    //����� ���������� ��������
    public void AddResurs()
    { 
        _resursCounter++;
        _drowCounter.DrowCountResurs(_resursCounter);
    }

    //����� ����������� ��������
    public void Resource�onsumption(int resurses)
    { 
        _resursCounter -= resurses;
        _drowCounter.DrowCountResurs(_resursCounter);
    }

    //����� ��������� ������ ���������� ��������
    public int GetResursCount()
    { 
        return _resursCounter;
    }
}
