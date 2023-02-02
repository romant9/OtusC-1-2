using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class VariablesClass : MonoBehaviour
{
    public static VariablesClass VC { get; private set; }

    public int speedKmh;
    private float speedMs;

    private float fuel;

    public float thinknessM;
    private double thinknessNm;

    public float priceRub;
    private uint heightOfRub;
    
    public sbyte carYear;

    public int enginPowerH;
    public int enginPowerKwt;

    public float lapLengthKm;
    private float distanceMax;

    private int lapsNumber;

    private int decomposeTimeYears;

    public Text CanvasMessage;
    public Text CanvasLog;
    private byte parCount;
    const string preError = "Введено неверное значение ";

    NumberStyles style = NumberStyles.AllowDecimalPoint;
    CultureInfo culture = CultureInfo.InvariantCulture;

    string dir;
    const string fileName = "Myparametres.txt";
    public class Parametr
    {
        public string name;
        public string value;
        public Text convertText;
        public bool isError;
    }

    List<Parametr> ParList = new List<Parametr>();

    void Awake()
    {
        VC = this;
        dir = Application.dataPath + "\\Resources\\Saves\\";
    }

    private float SpeedConvert(int speed)
    {       
        return Convert.ToSingle(speedKmh * 1000 / 3600);
    }
    private double ThinknessConvert(float thinkness)
    {
        return Convert.ToDouble(thinkness * 1e9);
    }
    private uint PriceConvert(float price)
    {
        return Convert.ToUInt32(price / 1.5f * 1000);
    }
    private sbyte CarYearConvert(int year)
    {
        return (sbyte)(DateTime.Today.Year - year);
    }
    private int EnginePowerConvert(int power)
    {
        return (int)MathF.Round(power * .73549875f);
    }
    public int LapsNumber(float distance, float lapLengthKm)
    {
        return (int)(distance/lapLengthKm);
    }
    public float DistanceMax(float speed, int power, float fuel)
    {
        //КПД
        const float kpd = .4f;
        //удельная теплота сгорания бензина
        const double q = 46 * 1e6;
        //расход бензина 1 км
        float fuelKm = power * 1000 * 1000 / (kpd * speed * (float)q);
        float distance = fuel / fuelKm;
        return distance;
    }

    private int DecomposeTime(double paintLayerThinknessNm, sbyte carYearDigit)
    {
        const double thinkForYear = 1.4275 * 1e5;
        return (int)(Math.Round(paintLayerThinknessNm / thinkForYear) - carYearDigit);
    }

    public void CalculateText()
    {
        speedMs = SpeedConvert(speedKmh);
        enginPowerKwt = EnginePowerConvert(enginPowerH);
        distanceMax = DistanceMax(speedMs, enginPowerKwt, fuel);
        lapsNumber = LapsNumber(distanceMax, lapLengthKm);
        thinknessNm = ThinknessConvert(thinknessM);
        decomposeTimeYears = DecomposeTime(thinknessNm, carYear);

        string time = decomposeTimeYears + " лет";
        string content = "Ваша машина развивает скорость "
            + speedMs + @"м\с" + "\n"
            + "Она может проехать " + lapsNumber + " кругов,\n"
            + "пройдя дистанцию " + distanceMax + " км\n"
            + "Ваша машина полностью сгниет через " + time + "\n"
            + "Счастливого пути!";
        CanvasMessage.text = content;
    }
    
    public void SaveFile()
    {
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        string path = dir + fileName;
        StreamWriter sw = new StreamWriter(path);
        sw.Write(CanvasMessage.text);
        sw.Close();
        CanvasLog.text = "Сохранили файл " + Path.GetFullPath(path);
    }

    public void LoadFile()
    {
        string path = dir + fileName;
        if (File.Exists(path))
        {
            StreamReader sr = new StreamReader(path);
            CanvasMessage.text = sr.ReadToEnd();
            CanvasLog.text = "Загрузили файл " + Path.GetFullPath(path);
        }
        else
        {
            CanvasMessage.text = "";
            CanvasLog.text = "Проверьте путь " + path;
        }
    }

    public Text GetText(InputField input)
    {
        return input.transform.parent.Find("Convert").GetComponent<Text>();
    }
    public string FindParName(InputField input)
    {
        return input.transform.parent.name;
    }

    public void SaveNewParameter(InputField input)
    {
        string parametrName = FindParName(input);
        var p = ParList.Find(x => x.name == parametrName);
        
        if (p != null)
        {
            p.value = input.text;
            p.isError = true;
            SwitchParametr(p);
        }
        else
        {
            ParList.Add(new Parametr
            {
                name = parametrName,
                value = input.text,
                convertText = GetText(input),
                isError = true,
            });
            parCount++;
            SwitchParametr(ParList.Last());
        }            
    }
    public void SwitchParametr(Parametr par)
    {
        CanvasLog.text = "Число параметров: " + parCount.ToString();
        switch (par.name)
        {
            case "Speed":
                ConvertSpeed(par);
                break;
            case "Fuel":
                ConvertFuel(par);
                break;
            case "Thinkness":
                ConvertThinkness(par);
                break;
            case "Price":
                ConvertPrice(par);
                break;
            case "Year":
                ConvertYear(par);
                break;
            case "Power":
                ConvertPower(par);
                break;
            case "LapLength":
                ConvertLapLength(par);
                break;
            default:
                CanvasLog.text = "Ошибка в имени переменной";               
                break;
        }        
    }
    private void ConvertSpeed(Parametr par)
    {
        if (int.TryParse(par.value, style, culture, out speedKmh))
        {
            par.convertText.text = SpeedConvert(speedKmh).ToString() + " м/c";
            par.isError = false;
        }
        else
        {
            par.isError = true;
            par.convertText.text = preError + par.value 
                + "\n" + "Введите целое число от 0 до 1.079252850e9";
        }
    }
    private void ConvertFuel(Parametr par)
    {
        if (float.TryParse(par.value, style, culture, out fuel))
        {
            par.convertText.text = fuel.ToString() + " л";
            par.isError = false;
        }
        else
        {
            par.isError = true;
            par.convertText.text = preError + par.value
                + "\n" + "Введите дробное число от 0 до 1e6";
        }       
    }
    private void ConvertThinkness(Parametr par)
    {
        par.isError = true;
        string error = preError + par.value
                + "\n" + "Введите дробное число от 0 до 1";       
        if (float.TryParse(par.value, style, culture, out thinknessM))
        {
            if (thinknessM < 1)
            {
                par.isError = false;
                par.convertText.text = ThinknessConvert(thinknessM).ToString() + " нм";

            }
            else par.convertText.text = error;
        }
        else
        {
            par.convertText.text = error;
        }
    }
    private void ConvertPrice(Parametr par)
    {
        if (float.TryParse(par.value, style, culture, out priceRub))
        {
            par.convertText.text = PriceConvert(priceRub).ToString() + " метров. Такова высота башни из рублей";
            par.isError = false;
        }
        else
        {
            par.isError = true;
            par.convertText.text = preError + par.value
                + "\n" + "Введите дробное число от 0 до 1e8";
        }
    }
    private void ConvertYear(Parametr par)
    {
        int year;
        if (int.TryParse(par.value, style, culture, out year))
        {
            par.convertText.text = CarYearConvert(year).ToString() + " лет вашей машине";
            par.isError = false;
        }
        else
        {
            par.isError = true;
            par.convertText.text = preError + par.value
                + "\n" + "Введите целое число от 0 до 256";
        }
    }
    private void ConvertPower(Parametr par)
    {
        if (int.TryParse(par.value, style, culture, out enginPowerH))
        {
            par.convertText.text = EnginePowerConvert(enginPowerH).ToString() + " КВт";
            par.isError = false;
        }
        else
        {
            par.isError = true;
            par.convertText.text = preError + par.value
                + "\n" + "Введите целое число от 0 до 1e6";
        }
    }
    private void ConvertLapLength(Parametr par)
    {
        if (float.TryParse(par.value, style, culture, out lapLengthKm))
        {
            par.convertText.text = (lapLengthKm * 1000).ToString() + " м";
            par.isError = false;
        }
        else
        {
            par.isError = true;
            par.convertText.text = preError + par.value
                + "\n" + "Введите дробное число от 0 до 1e6";
        }
    }

}
