using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//Скрипт командного центра Базы
public class CommandCenter : MonoBehaviour
{
    [SerializeField] private Scaner _scaner;    //Компонент сканера(скрипт)
    [SerializeField] private Drone _drone;    //Прифаб дронов
    [SerializeField] private Transform _spawnPositionDrone;    //Место спавно нового дрона
    [SerializeField] private Transform _dronsContainer;         //Контейнер дронов
    [SerializeField] private Transform[] _pointPatrolling;    //Точка патрулирования

    private Queue<Resurs> _resursers = new Queue<Resurs>(); //Список ресурсов Базы    
    private Queue<Drone> _drons = new Queue<Drone>();   //Cписок дронов
    private Drone _tempDrone;   //Временный дрон
    private bool _isHaveDrone = false;   //Дрон создан
    private int _resurseStorage = 0;    //Хранилище ресурсов    

    private void Update()
    {
        //Проверяем что нажата клавиша сканирования
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _scaner.Scane(_resursers);
            //Debug.Log("Найдено ресурсов " + _resursers.Count);
        }

        //Создаём дрона по нажатию на Е
        if (Input.GetKeyDown(KeyCode.E) && !_isHaveDrone)
        {
            CreateDrons();
            _isHaveDrone = true;
        }

        //Отправляем дрона за ресурсами
        if (_drons.Count > 0)
        {
            if (_resursers.Count > 0)
            {
                SendDrone();
            }
        }
    }

    //Метод создания дронов
    private void CreateDrons()
    { 
        _tempDrone = Instantiate(_drone, _spawnPositionDrone.position, Quaternion.identity,_dronsContainer);    //Создаём дрона
        _tempDrone.SetCommandCenterPosition(this.transform);    //Указываем родительский командный центр
        _tempDrone.SetCommandCenter(this);    //Указываем родительский командный центр
        _tempDrone.SetPointPatrolling(_pointPatrolling);    //Указываем точки патрулирования
        _drons.Enqueue(_tempDrone); //Добавляем дрона в список дронов
    }

    //Метод отправки дрона
    private void SendDrone()
    {           
        _tempDrone = _drons.Dequeue();  //Берём дрона из очереди
        _tempDrone.SetTarget(_resursers.Dequeue().transform);   //Задание цели для дрона        
    }

    //Метод получения ресурса с указанием дрона который принёс ресурс
    public void TakeResurs(Drone drone)
    {
        _resurseStorage++;  //Увеличиваем количество ресурсов в базе
        _drons.Enqueue(drone);  //Помещаем дрона в очередь

        //Если ресурсов на базе более трёх
        if (_resurseStorage >= 3)
        {            
            CreateDrons();  //Создаём дрона
            _resurseStorage -=3;    //Расходуем ресурсы
        }        
        Debug.Log("Ресурсов в базе: " + _resurseStorage);
    }
}
