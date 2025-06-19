using UnityEngine;
using UnityEngine.UI;

//Скрипт отображения количества ресурсов
public class DrowCounter : MonoBehaviour
{
    [SerializeField] private Text _text;    //Компонент текста

    //Метод рисования
    public void DrowCountResurs(int count)
    { 
        _text.text = count.ToString();  //Отрисовываем количество ресурсов у базы
    }
}
