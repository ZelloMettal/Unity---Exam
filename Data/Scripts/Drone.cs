using System.Drawing;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

//Скрипт дрона
public class Drone : MonoBehaviour
{
    [SerializeField] private float _speed;  //Скорость дрона
    [SerializeField] Transform _commandCenterPosition;  //Позиция командного центра
    [SerializeField] private Transform _resursPositionOnDrone;  //Позиция ресурса ан дроне

    Transform[] _pointPatrolling;  //Массив точек патрулирования
    private int _currentPoint = 0;  //Начальная точка патрулирования
    private bool _isReady = false;  //Готовность дрона
    private bool _isHaveTarget = false; //Есть ли у дрона цель
    private bool _isHaveResurses = false;   //Есть ли у дрона ресурс
    private Transform _target;  //Текущая цель дрона
    private CommandCenter _commandCenter; //Командный центер дрона
    private Resurs _tempResurs; //Временный контейнера ресурса

    private void Update()
    {
        if (_isReady)
        {
            if (_isHaveTarget)
            {
                //Если есть цель то он двигается к цели
                MoveToTarget(_target);
            }
            else 
            {
                if (_isHaveResurses)
                {                    
                    //Возвращаемся на базу 
                    MoveToTarget(_commandCenterPosition);
                }
                else
                { 
                    //Двигаемся свободно
                    FreeMove();  
                }
            }
        }    
    }

    //Метод патрулирования дрона
    private void FreeMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, _pointPatrolling[_currentPoint].position, _speed* Time.deltaTime); //Перемещаемся к точки патрулированию
        transform.LookAt(_pointPatrolling[_currentPoint].position); //Поворачиваемся к точки патрулирования
    }

    //Триггер достижения точки патрулирования
    private void OnTriggerEnter(Collider other)
    {
        //Проверяем достигли дрон точки патрулирования
        if (other.gameObject.TryGetComponent(out PointPatrolling point))
        { 
            _currentPoint = ++_currentPoint % _pointPatrolling.Length;  //Перемещаемся в массиве точек
        }    
    }

    //Метод движения к базе
    private void MoveToTarget(Transform target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime); //Перемещаемся к целевой точки
        transform.LookAt(target.position);  //Поворачиваемся к целевой точки
    }

    //Метод поднятия ресурса
    private void Drag()
    {
        _isHaveResurses = true; //Есть ресурс у дрона
        _isHaveTarget = false;  //Нет цели
        _tempResurs.PrepereDrag();  //Подготовка ресурса к поднятию
        _tempResurs.transform.position = _resursPositionOnDrone.position;   //Помещаем ресурс в ячейку над дроном
        _tempResurs.transform.SetParent(this.transform);    //Назначем родителя ресурсу
    }

    //Метод отпускания ресурса
    private void Drop()
    {
        _isHaveResurses = false;    //Нет ресурса у дрона        
        Destroy(_tempResurs.gameObject);    //Уничтожаем ресурс после сброса
        _tempResurs = null; //Очищаем контейнер под ресурс
        _commandCenter.TakeResurs(this);    //Передаём ресурс командному центру с указанием дрона
    }

    //Ивент сброса ресурса базе
    private void OnTriggerStay(Collider collision)
    {
        //Если мы столкнулись с базой и у нас есть ресурс
        if (collision.gameObject.TryGetComponent<CommandCenter>(out CommandCenter center) && _isHaveResurses)
        {
            Drop(); //Сбрасываем ресурс
        }
    }

    //Ивент сбора ресурса
    private void OnCollisionEnter(Collision collision)
    {
        //Проверяем столкнулся ли дрон с ресурсом у нас нет ресурса и нет цели
        if (collision.gameObject.TryGetComponent<Resurs>(out Resurs resurs) && !_isHaveResurses)
        {
            //Проверяем достиг ли дрон цели
            if (_target.position == resurs.transform.position)
            { 
                _tempResurs = resurs;   //Получаем ресурс
                Drag(); //Подбираем ресурс
            }
        }
    }

    //Метод установки позиции командного центра
    public void SetCommandCenterPosition(Transform position)
    { 
        _commandCenterPosition = position;
    }

    //Метод установки командного центра
    public void SetCommandCenter(CommandCenter center)
    {
        _commandCenter = center;
    }

    //Метод установки точек патрулирования
    public void SetPointPatrolling(Transform[] patrolling)
    {
        _pointPatrolling = patrolling;
        _isReady = true;
    }

    //Установка цели для дрона
    public void SetTarget(Transform target)
    { 
        _target = target;
        _isHaveTarget = true;
    } 
}
