using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

//Скрипт Сканера области
public class Scaner : MonoBehaviour
{
    [SerializeField] private float _scaneRadius;    //Радиус области сканирования

    //Рисуем область сканирования ресурсов
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _scaneRadius);
    }

    //Метод сканирования принимает список(очередь) ресурсов и возвращает новый список(очередь) ресурсов
    public Queue<Resurs> Scane(Queue<Resurs> resurses)
    {
        //Собираем все коллайдеры которые попали в область сканирования
        Collider[] triggerColliders = Physics.OverlapSphere(transform.position, _scaneRadius);
        
        //Перебираем полученные коллайдеры
        foreach (Collider collider in triggerColliders) 
        {
            //Ищем коллайдеры с компонентом Resus 
            if (collider.gameObject.TryGetComponent<Resurs>(out Resurs resurs))
            {
                //Проверяем есть ли ресурс в списке ресурсов
                if (!resurses.Contains(resurs))
                {
                    //Проверяем доступен ли ресурс для сбора
                    if (!resurs.IsIncludeFree)
                    { 
                        resurs.SetInclude();    //Помечаем ресурс как доступен для сбора
                        resurses.Enqueue(resurs);   //Помещаем ресурс в список ресурсов
                    }
                }
            }
        }

        return resurses;
    }    
}
