using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectionsClass : MonoBehaviour
{

    //1.������� ���������� ����, � ����������� �� �� �������� ������� ������ int\float
    //2.������� ���������� ���� int\float. ��������� ������ ���, ����� ������ ������� ��� ��������� �����������.
    //3.������� � �������� ���������� (������������ ��������)
    //4.������� ������� �� ���� �������� ���������� (������� � ������� 2). ����� ���������� �������.������� ����� 3\4
    //5.������� ������� �� ���� �������� ���������� REF(������� � ������� 2). ����� ���������� �������.������� ����� 3\4
    //6.������� ������� �� ���� �������� ���������� OUT(������� � ������� 2). ����� ���������� �������.������� ����� 3\4
    //7.�������� ���������, ������� ����� ��������� ��� �������� ��� ���������� �������.
    //8.�������� �� � ���� ��� � ������ �������.
    //9.�������� �� � ���� ����� ������������.
    //10.������� ������ �� �����.
    //���.�������
    //11.������� ������ ��� ����������� ���� �������� �� ����

    [Flags]
    public enum Types
    {
        Mage,
        Warrior,
        Archer,

    }
    public Types _types = Types.Mage;

    public HashSet<string> hash = new HashSet<string>();

    private int lifesCount;
    public Action<int> onLifeChange;
    //event - ������ ������� Invoke �� ������� ������
    //public event Action<int> onLifeChange;

    void Start()
    {
        onLifeChange += LifeChange;
    }

    void Update()
    {
        
    }
    float CurrentSpeed(float speed)
    {
        return speed;
    }
    private void OnHit()
    {
        lifesCount--;
        onLifeChange.Invoke(lifesCount);
    }
    public void LifeChange(int lifes)
    {
        Debug.Log(lifesCount);
    }
}
