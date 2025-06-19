using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

//Скрипт ресурса
public class Resurs : MonoBehaviour
{
    //Контейнеры компонентов
    private Rigidbody _rigidbody;
    private BoxCollider _boxCollider;

    public bool IsIncludeFree { get; private set; } = false;    //Свойство состояния доступности ресурса для сбора    

    void Start()
    {
        //Получаем компоненты
        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    //Метод сброса настроек ресурса  
    public void StandartSetting()
    { 
        IsIncludeFree = false;
    }

    //Метод установки состояния для сбора
    public void SetInclude()
    {
        IsIncludeFree = true;
    }

    //Метод взятия куба
    public void PrepereDrag()
    {
        _rigidbody.useGravity = false; //Отключаем гравитацию
        _rigidbody.isKinematic = true; //Делаем куб неподвижным
        _boxCollider.enabled = false;  //Отключаем коллайдер чтобы ресурс не передавался между дронами
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; //Кализия куба в руках
    }
}
