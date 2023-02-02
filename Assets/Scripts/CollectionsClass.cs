using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectionsClass : MonoBehaviour
{

    //1.Создать переменную булл, в зависимости от ее значения создать массив int\float
    //2.Создать переменную типа int\float. Заполнить массив так, чтобы каждый елемент был квадратом предыдущего.
    //3.Создать и отловить исключение (переполнение значения)
    //4.Создать функцию на вход передать переменную (которая в задании 2). После выполнения функции.Сделать пункт 3\4
    //5.Создать функцию на вход передать переменную REF(которая в задании 2). После выполнения функции.Сделать пункт 3\4
    //6.Создать функцию на вход передать переменную OUT(которая в задании 2). После выполнения функции.Сделать пункт 3\4
    //7.Объявить структуру, которая будет содержать все элементы для предыдущих заданий.
    //8.Записать ее в файл как в первом занятии.
    //9.Записать ее в файл через сериализацию.
    //10.Считать данные из файла.
    //доп.задание
    //11.Создать кнопки для управлением всем процесом из игры

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
    //event - нельзя вызвать Invoke из другого класса
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
