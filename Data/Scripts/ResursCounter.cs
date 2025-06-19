using System;
using UnityEngine;

//Скрипт счётчика ресурсов у базы
public class ResursCounter : MonoBehaviour
{
    [SerializeField] DrowCounter _drowCounter;  //Объект рисования счётчика

    private int _resursCounter = 0; //Количество ресурсов

    //Метод добавления ресурсов
    public void AddResurs()
    { 
        _resursCounter++;
        _drowCounter.DrowCountResurs(_resursCounter);
    }

    //Метод потребления ресурсов
    public void ResourceСonsumption(int resurses)
    { 
        _resursCounter -= resurses;
        _drowCounter.DrowCountResurs(_resursCounter);
    }

    //Метод получения общего количества ресурсов
    public int GetResursCount()
    { 
        return _resursCounter;
    }
}
