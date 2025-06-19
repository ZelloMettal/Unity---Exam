using System.Collections;
using UnityEngine;

//Скрипт спавнера ресурсов
public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform _startPointSwanPosition; //Стартовая точка зоны спавнера
    [SerializeField] private Transform _endPointSwanPosition;   //Кончная точка зоны сканирования
    [SerializeField] private Transform _container;  //Контейнер заспавненых ресурсов
    [SerializeField] private Resurs _prefabResurs;  //Префаб ресурса
    [SerializeField] private float _delaySpawn; //Время задержки спавна ресурса

    private WaitForSeconds _wait;   //Ожидание спавна 

    private void Start()
    {
        _wait = new WaitForSeconds(_delaySpawn);    //Задаём время ожидания спавна
        StartCoroutine(SpawnResurs());  //Запускаем корутину спавнера ексурсов
    }

    //Метод спавна ресурсов
    private IEnumerator SpawnResurs()
    {
        while (enabled)
        {
            //Создаём объект Ресурса в случайных координатах и помещаем в контейнер
            Instantiate(_prefabResurs, 
                new Vector3( 
                    Random.Range(_startPointSwanPosition.position.x, _endPointSwanPosition.position.x), 
                    2, 
                    Random.Range(_startPointSwanPosition.position.z, _endPointSwanPosition.position.z)), 
                    Quaternion.identity, 
                    _container);
            yield return _wait; //Ожидаем
        }
    }
}
