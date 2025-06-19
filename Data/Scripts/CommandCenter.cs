using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(MeshRenderer))]

//Скрипт командного центра Базы
public class CommandCenter : MonoBehaviour
{
    [SerializeField] private Scaner _scaner;    //Компонент сканера(скрипт)
    [SerializeField] private Drone _drone;    //Прифаб дронов
    [SerializeField] private Transform _spawnPositionDrone;    //Место спавно нового дрона
    [SerializeField] private Transform _dronsContainer;         //Контейнер дронов
    [SerializeField] private Transform[] _pointPatrolling;    //Точка патрулирования
    [SerializeField] private ResursCounter _resursCounter;    //Точка патрулирования
    [SerializeField] private Color _defauldColor;    //Цвет базы по умолчанию

    private MeshRenderer _meshRenderer; //Контейнер компонента
    private Queue<Resurs> _resursers = new Queue<Resurs>(); //Список ресурсов Базы    
    private Queue<Drone> _drons = new Queue<Drone>();   //Cписок дронов
    private Drone _tempDrone;   //Временный дрон
    private bool _isHaveDrone = false;   //Дрон создан
    private Resurs _tempResurs; //Веременный ресурс
    private bool _isBuilding = false;   //Состояние строительства базы
    private bool _isReady = true;  //Состояние готовности базы
    private int _resurseCountForCreateBase = 5;     //Количество ресурсов для создания базы
    private int _resurseCountForCreateDrone = 3;    //Количество ресурсов для создания дрона
    private CommandCenter _childrenBase;    //Контейнер дочерней базы

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();   //Получаем компонент   
    }

    private void Update()
    {        
        //проверяем что база готова
        if (_isReady)
        {
            //Проверяем что база в режиме строительства
            if (!_isBuilding)
            {
                ScanResurses();
                //Создаём дрона по нажатию на Е
                if (Input.GetKeyDown(KeyCode.E) && !_isHaveDrone)
                {
                    CreateDrons();
                    _isHaveDrone = true;
                }                
            }
            else 
            {
                ScanResurses();
                //Проверяем хватает ли дронов и ресурсов для строительства новой базы
                if (_drons.Count > 1 && _resursCounter.GetResursCount() >= _resurseCountForCreateBase)
                {
                    SendDroneForBuild();            
                }
            }
        }
    }

    //Метод создания дронов
    private void CreateDrons()
    { 
        _tempDrone = Instantiate(_drone, _spawnPositionDrone.position, Quaternion.identity, _dronsContainer);    //Создаём дрона
        _tempDrone.SetCommandCenterPosition(this.transform);    //Указываем родительский командный центр
        _tempDrone.SetCommandCenter(this);    //Указываем родительский командный центр
        _tempDrone.SetPointPatrolling(_pointPatrolling);    //Указываем точки патрулирования
        _drons.Enqueue(_tempDrone); //Добавляем дрона в список дронов
    }

    //Метод отправки дрона за ресурсами
    private void SendDroneForResurses()
    {           
        _tempDrone = _drons.Dequeue();  //Берём дрона из очереди        
        _tempResurs = _resursers.Dequeue(); //Получаем ресурс из очереди
        _tempResurs.StandartSetting();  //Помечаем ресурс как не доступен для сбора
        _tempDrone.SetTarget(_tempResurs.transform);   //Задание цели для дрона        
    }

    //Метод отправки дрона на строитешльство базы
    private void SendDroneForBuild()
    {
        _tempDrone = _drons.Dequeue();  //Берём дрона из очереди 
        _tempDrone.SetTarget(_childrenBase.transform);   //Задание цели для дрона
        _resursCounter.ResourceСonsumption(_resurseCountForCreateBase); //Потребляем ресурсы
        _isBuilding = false;    //Выходим из режима строительства
    }

    //Ивент получения дрона для строительства от родительской базы
    private void OnTriggerEnter(Collider collision)
    {
        //Проверяем столкнулась ли база с дроном в режиме строительства и база не готова
        if (collision.gameObject.TryGetComponent<Drone>(out Drone drone) && _isBuilding == true && _isReady == false)
        {
            this._isBuilding = false;   //Сбрасываем состояние строительства
            this._isReady = true;   //Устанавливаем состояние готовности
            ChangeColor(_defauldColor);  //Устанавливаем цвет по умолчанию
            Destroy(drone.gameObject);  //Уничтожаем дрона родительской базы
            CreateDrons();  //Создаём нового дрона
        }
    }

    //Метод установки состояния готовности
    private void SetIsReady(bool isReady)
    {
        _isReady = isReady;
    }

    //Метод установки состояния строительства
    private void SetIsBuild(bool isBuild)
    {
        _isBuilding = isBuild;
    }

    //Мотод сканирования ресурсов
    private void ScanResurses()
    {
        //Проверяем что нажата клавиша сканирования
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _scaner.Scane(_resursers);
        }
        //Отправляем дрона за ресурсами
        if (_drons.Count > 0)
        {
            if (_resursers.Count > 0)
            {
                SendDroneForResurses();
            }
        }
    }

    //Метод изменения цвета базы
    public void ChangeColor(Color color)
    {
        Material material = _meshRenderer.material;
        material.color = color;
        _meshRenderer.material = material;
    }

    //Метод получения ресурса с указанием дрона который принёс ресурс
    public void TakeResurs(Drone drone)
    {
        _resursCounter.AddResurs();  //Увеличиваем количество ресурсов в базе
        _drons.Enqueue(drone);  //Помещаем дрона в очередь

        //Проверяем хватает ли ресурсов для создания нового дрона
        if (_resursCounter.GetResursCount() >= _resurseCountForCreateDrone && _isBuilding != true)
        {
            CreateDrons();
            _resursCounter.ResourceСonsumption(_resurseCountForCreateDrone);
        }
    }

    //Метод начала строительства базы
    public void StartBuildNewBase(CommandCenter newBasePosition, bool isBuild)
    {
        this._isBuilding = isBuild;     //Устанавливаем состояние строительства для родительской базы
        _childrenBase = newBasePosition;    //Получаем объект дочерней базы
        _childrenBase.SetIsReady(false);    //Сбрасываем состояние готовности для дочерней базы
        _childrenBase.SetIsBuild(true);  //Устанавливаем режим строительства для дочерней базы
    }

    //Метод получения количество дронов
    public int GetDronCount()
    {
        return _drons.Count;
    }        
}
