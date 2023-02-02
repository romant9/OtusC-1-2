using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CollectionsTest : MonoBehaviour
{
    //false - int, true - float
    [SerializeField] private bool arrayType;
    [SerializeField] private int itemNumber;

    [SerializeField] private float arrayItemAbstract;
    private int arrayItemInt;
    private float arrayItemFloat;

    private int[] arrayInt;
    private float[] arrayFloat;

    private Action<int> CreateArrayInt;
    private Action<float> CreateArrayFloat;

    [SerializeField] private Text CanvasMessage;
    [SerializeField] private string MessageTextAdditive;
    //[SerializeField] private bool messageTextFromStruct;
    [SerializeField] private Text CanvasLog;

    [SerializeField] private CollectAllArrays MyStruct;
    private bool HasMyStruct;

    [SerializeField] private bool isSerializeFile;

    private string exceptionStr = "";
    const string preError = "Введено неверное значение ";

    NumberStyles style = NumberStyles.AllowDecimalPoint;
    CultureInfo culture = CultureInfo.InvariantCulture;

    string dir;
    const string fileName = "Mycollections.txt";

    void Start()
    {
        dir = Application.dataPath + "\\Resources\\Saves\\";

        CreateArrayInt += IsInvokedInt;
        CreateArrayFloat += IsInvokedFloat;
    }

    public Text GetText(Transform input)
    {
        return input.parent.Find("Convert").GetComponent<Text>();
    }

    public void ArrayType(Toggle tg)
    {
        arrayType = tg.isOn;
        GetText(tg.transform).text = arrayType ? "float" : "int";
    }

    public void SwitchSerialize(Toggle tg)
    {
        isSerializeFile = tg.isOn;
    }

    //public void MessageTextSource(Toggle tg)
    //{
    //    messageTextFromStruct = tg.isOn;
    //    GetText(tg.transform).text = messageTextFromStruct ? "из структуры" : "из текущего класса";
    //}

    public void SetArrayNumber(InputField input)
    {
        if (int.TryParse(input.text, style, culture, out itemNumber))
        {
            GetText(input.transform).text = itemNumber.ToString() + " членов массива";
        }
        else
        {
            GetText(input.transform).text = preError + input.text
                + "\n" + "Введите целое число от 0 до 100";
        }
    }
    public void SetItemValue(InputField input)
    {
        if (float.TryParse(input.text, style, culture, out arrayItemAbstract))
        {
            GetText(input.transform).text = arrayItemAbstract.ToString();
        }
        else
        {
            GetText(input.transform).text = preError + input.text
                + "\n" + "Введите целое число от 0 до 100";
        }
    }

    //рекурсия для возведения в степень
    //не работает
    public float PowItem(float item)
    {
        if ((double)item * item > float.MaxValue)
        {
            return item;
        }
        return PowItem((float)Math.Pow(item, 2));
    }

    //вызываем по кнопке onClick
    public void BtCreateArray()
    {
        MessageTextAdditive = "";
        CanvasMessage.text = "";
        exceptionStr = "";

        if (!arrayType)
        {
            arrayItemInt = (int)arrayItemAbstract;
            CreateArrayInt.Invoke(arrayItemInt);
        }
        else 
        {
            arrayItemFloat = arrayItemAbstract;
            CreateArrayFloat.Invoke(arrayItemFloat);
        }
    }
    private void IsInvokedInt(int item)
    {
        HasMyStruct = false;
        if (itemNumber > 0 && item != 0)
        {
            var arrayIntList = new List<int>();
            for (int i = 0; i < itemNumber; i++)
            {
                if (Math.Abs(item) < int.MaxValue && item != 0)
                {
                    arrayIntList.Add(item);
                    MessageTextAdditive += i + " : " + arrayIntList[i] + "\n";
                    Debug.Log("int " + arrayIntList[i]);
                }
                else
                {
                    if (exceptionStr == "")
                    {
                        exceptionStr = "Последний элемент массива имеет значение " + arrayIntList[i - 1] + " и индекс " + (i-1);
                    }
                    Debug.Log("Exception ");
                    break;
                }

                try
                {
                    item *= item;
                }
                catch (OverflowException)
                {
                    Debug.Log("Exception " + i + " : " + item);    //не работает
                }
            }

            arrayInt = arrayIntList.ToArray();
            arrayIntList.Clear();

            MessageTextAdditive += "Тип массива: int" + "\n" + "Всего элементов: " + arrayInt.Length + "\n";
            if (exceptionStr != "")
            {
                MessageTextAdditive += exceptionStr;
            }
            CanvasMessage.text = MessageTextAdditive;
            MyStruct = new CollectAllArrays(arrayType, arrayInt.Length, arrayItemInt, arrayItemFloat, arrayInt, arrayFloat);
            HasMyStruct = true;
        }       
    }
    private void IsInvokedFloat(float item)
    {
        HasMyStruct = false;
        if (itemNumber > 0 && item != 0)
        {
            var arrayFloatList = new List<float>();
            for (int i = 0; i < itemNumber; i++)
            {
                if (Math.Abs(item) < float.MaxValue && item != 0)
                {
                    arrayFloatList.Add(item);
                    MessageTextAdditive += i + " : " + arrayFloatList[i] + "\n";
                    Debug.Log("float " + arrayFloatList[i]);
                }
                else
                {
                    if (exceptionStr == "")
                    {
                        exceptionStr = "Последний элемент массива имеет значение " + arrayFloatList[i-1] + " и индекс " + (i-1);
                    }
                    Debug.Log("Exception ");
                    break;
                    
                }
                item = (float)Math.Pow(item, 2);
            }

            arrayFloat = arrayFloatList.ToArray();
            arrayFloatList.Clear();

            MessageTextAdditive += "Тип массива: float" + "\n" +  "Всего элементов: " + arrayFloat.Length + "\n";
            if (exceptionStr != "")
            {
                MessageTextAdditive += exceptionStr;
            }
            CanvasMessage.text = MessageTextAdditive;
            MyStruct = new CollectAllArrays(arrayType, arrayFloat.Length, arrayItemInt, arrayItemFloat, arrayInt, arrayFloat);
            HasMyStruct = true;
        }
    }

    //определяем структуру
    public struct CollectAllArrays
    {
        public bool arrayType;
        public int itemNumber;

        public int arrayItemInt;
        public float arrayItemFloat;

        public int[] arrayInt;
        public float[] arrayFloat;

        public CollectAllArrays(bool arrayType, int itemNumber, int arrayItemInt, float arrayItemFloat, int[] arrayInt, float[] arrayFloat)
        {
            this.arrayType = arrayType;
            this.itemNumber = itemNumber;
            this.arrayItemInt = arrayItemInt;
            this.arrayItemFloat = arrayItemFloat;
            this.arrayInt = arrayInt;
            this.arrayFloat = arrayFloat;
        }
    }

    public void SaveFile()
    {
        if (HasMyStruct)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string path = dir + fileName;

            if (isSerializeFile)
            {
                string myStruct = JsonUtility.ToJson(MyStruct);

                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    writer.Write(myStruct);
                }
            }
            else
            {
                StreamWriter sw = new StreamWriter(path);

                sw.WriteLine(MyStruct.arrayType ? "float" : "int");
                sw.WriteLine(MyStruct.itemNumber);
                sw.WriteLine(MyStruct.arrayType ? MyStruct.arrayItemFloat : MyStruct.arrayItemInt);
                if (MyStruct.arrayType)
                    sw.WriteLine(string.Join("\n", MyStruct.arrayFloat));
                else
                    sw.WriteLine(string.Join("\n", MyStruct.arrayInt));
                sw.Close();
            }
            CanvasLog.text = "Сохранили файл " + Path.GetFullPath(path);
        }
        else
        {
            CanvasLog.text = "Структура отсутствует";
        }
    }

    public void LoadFile()
    {
        string path = dir + fileName;
        if (File.Exists(path))
        {
            if (isSerializeFile)
            {
                using (var sr = new StreamReader(path))
                {
                    MyStruct = JsonUtility.FromJson<CollectAllArrays>(sr.ReadToEnd());
                    
                }
                string Content = "Сериализация включена\n"
                        + "Загрузили структуру:\n";

                arrayType = MyStruct.arrayType;
                itemNumber = MyStruct.itemNumber;
                arrayItemInt = MyStruct.arrayItemInt;
                arrayItemFloat = MyStruct.arrayItemFloat;
                arrayInt = MyStruct.arrayInt;
                arrayFloat = MyStruct.arrayFloat;

                string array = "";
                if (arrayType)
                    array = string.Join("\n", MyStruct.arrayFloat);
                else
                    array = string.Join("\n", MyStruct.arrayInt);

                Content += (arrayType ? "float" : "int") + "\n"
                        + itemNumber + "\n"
                        + array;
                CanvasMessage.text = Content;
            }
            else
            {
                StreamReader sr = new StreamReader(path);
                CanvasMessage.text = "Сериализация выключена\n" 
                    + "Загрузили структуру:\n" + sr.ReadToEnd();
            }
            
            CanvasLog.text = "Загрузили файл " + Path.GetFullPath(path);
        }
        else
        {
            CanvasMessage.text = "";
            CanvasLog.text = "Проверьте путь " + path;
        }
    }

    
}
