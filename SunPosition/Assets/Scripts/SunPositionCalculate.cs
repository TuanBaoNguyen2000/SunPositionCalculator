using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class SunPositionCalculate : Singleton<SunPositionCalculate>
{
    [Header("INPUT")]
    public float latitude;
    public float longitide;
    public int day, month, year;
    public int hour, minute, second;

    [Header("Solar Time")]
    [SerializeField] private float LST; // Local solar time
    [SerializeField] private float LT; // local time 
    [SerializeField] private float LSTM; // Local standard time meridian
    [SerializeField] private int T_GMT; // Time zone
    [SerializeField] private int daysOfYear;
    [SerializeField] private float B; 
    [SerializeField] private float TC; // Time correction factor
    [SerializeField] private float HRA; // Hour Angle

    [Header("OUPUT")]
    public float EoT; // Equation of Time
    public float sunrise;
    public float sunset;
    public float declinationAngle;
    public float elevationAngle; 
    public float azimuthAngle;


    public float ElevationAngle()
    {
        return elevationAngle;
    }
    public float AzimuthAngle()
    {
        return azimuthAngle;
    }

    public void CalculateSunPosition()
    {
        Calculate(day, month, year);
    }

    private void Calculate(int day, int month, int year)
    {
        float latitude = this.latitude * Mathf.Deg2Rad;

        T_GMT = (int)longitide / 15;
        LSTM = 15 * T_GMT;

        daysOfYear = CalculateDayOfYear(day, month, year);
        B = (float)(360.0 / 365.0) * (daysOfYear - 81) * Mathf.Deg2Rad;
        EoT = (float)(9.87 * Mathf.Sin(2 * B) - 7.53 * Mathf.Cos(B) - 1.5 * Mathf.Sin(B));

        TC = 4 * (longitide - LSTM) + EoT;
        LT = (float)(hour + minute / 60.0 + second / 3600.0);
        LST = LT + TC / 60;
        HRA = 15 * (LST - 12) * Mathf.Deg2Rad;

        declinationAngle = (float)(23.45 * Mathf.Deg2Rad * Mathf.Sin(B));
        elevationAngle = Mathf.Asin(Mathf.Sin(declinationAngle) * Mathf.Sin(latitude) + Mathf.Cos(declinationAngle) * Mathf.Cos(latitude) * Mathf.Cos(HRA) );
        azimuthAngle = Mathf.Acos((Mathf.Sin(declinationAngle) * Mathf.Cos(latitude) - Mathf.Cos(declinationAngle) * Mathf.Sin(latitude) * Mathf.Cos(HRA)) / Mathf.Cos(elevationAngle));
        if (LST > 12 || HRA > 0)
            azimuthAngle = 2 * Mathf.PI - azimuthAngle;

        sunrise = 12 - (1 / (15 * Mathf.Deg2Rad)) * Mathf.Acos(-Mathf.Tan(latitude) * Mathf.Tan(declinationAngle)) - TC / 60;
        sunset = 12 + (1 / (15 * Mathf.Deg2Rad)) * Mathf.Acos(-Mathf.Tan(latitude) * Mathf.Tan(declinationAngle)) - TC / 60;

        declinationAngle = declinationAngle * Mathf.Rad2Deg;
        elevationAngle = elevationAngle * Mathf.Rad2Deg;
        azimuthAngle = azimuthAngle * Mathf.Rad2Deg;

    }


    private bool IsDayCorrect(int day, int month, int year)
    {
        if (day <= 0 || day > 31) return false;
        if (month <= 0 || month > 12) return false;
        if ((month == 4 || month == 6 || month == 9 || month == 11) && day >= 31) return false;
        if (month == 2 && day > 29) return false;
        if (!IsLeapYear(year) && month == 2 && day >= 29) return false;

        return true;
    }

    public int CalculateDayOfYear(int day, int month, int year)
    {
        int totalDays = 0;
        for (int i = 1;  i <= month; i++)
        {
            if (i != month)
                totalDays += DaysInMonth(i, year);
            else
                totalDays += day;
        }

        return totalDays;
    }

    private int DaysInMonth(int month, int year)
    {
        switch (month)
        {
            case 1: 
            case 3: 
            case 5: 
            case 7: 
            case 8: 
            case 10: 
            case 12: 
                return 31;
            case 4: 
            case 6: 
            case 9: 
            case 11: 
                return 30;
            case 2: 
                return IsLeapYear(year) ? 29 : 28;
            default:
                return 0;
        }
    }

    private bool IsLeapYear(int year)
    {
        return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
    }
}
