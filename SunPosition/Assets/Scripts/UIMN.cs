using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMN : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputField latitudeInput;
    [SerializeField] private InputField longitideInput;
    [SerializeField] private InputField yearInput;
    [SerializeField] private Dropdown dayInput, monthInput;
    [SerializeField] private Dropdown hourInput, minuteInput, secondInput;

    [Header("Output")]
    [SerializeField] private Text EotTxt;
    [SerializeField] private Text declinationAngleTxt;
    [SerializeField] private Text sunriseTxt, sunsetTxt;
    [SerializeField] private Text elevationAngleTxt, azimuthAngleTxt;

    private void Start()
    {
        AddDropDownOptions(dayInput, 1, 31);
        AddDropDownOptions(monthInput, 1, 12);
        AddDropDownOptions(hourInput, 0, 23);
        AddDropDownOptions(minuteInput, 0, 59);
        AddDropDownOptions(secondInput, 0, 59);
        EventSettting();
    }

    private void AddDropDownOptions(Dropdown dropdown, int startValue, int endValue)
    {
        dropdown.ClearOptions();
        List<string> options = new List<string>();

        for (int i = startValue; i <= endValue; i++)
        {
            options.Add(i.ToString());
        }

        dropdown.AddOptions(options);
    }

    private void EventSettting()
    {
        latitudeInput.onEndEdit.AddListener(delegate { UpdateInputValue(); UpdateOutputValue(); });
        longitideInput.onEndEdit.AddListener(delegate { UpdateInputValue(); UpdateOutputValue(); });

        yearInput.onEndEdit.AddListener(delegate { UpdateInputValue(); UpdateOutputValue(); });
        dayInput.onValueChanged.AddListener(delegate { UpdateInputValue(); UpdateOutputValue(); });
        monthInput.onValueChanged.AddListener(delegate { UpdateInputValue(); UpdateOutputValue(); });

        hourInput.onValueChanged.AddListener(delegate { UpdateInputValue(); UpdateOutputValue(); });
        minuteInput.onValueChanged.AddListener(delegate { UpdateInputValue(); UpdateOutputValue(); });
        secondInput.onValueChanged.AddListener(delegate { UpdateInputValue(); UpdateOutputValue(); });

    }

    private void HandleInputValueChanged(string text)
    {
        Debug.Log("New value InputField: " + text);
    }

    private void HandleDropdownValueChanged(Dropdown dropdown, int index)
    {
        string selectedOption = dropdown.options[index].text;
        Debug.Log("Option: " + selectedOption);
    }

    public void UpdateInputValue()
    {
        float.TryParse(latitudeInput.text, out SunPositionCalculate.Instance.latitude);
        float.TryParse(longitideInput.text, out SunPositionCalculate.Instance.longitide);

        SunPositionCalculate.Instance.day = int.Parse(dayInput.options[dayInput.value].text);
        SunPositionCalculate.Instance.month = int.Parse(monthInput.options[monthInput.value].text);
        int.TryParse(yearInput.text, out SunPositionCalculate.Instance.year);

        SunPositionCalculate.Instance.hour = int.Parse(hourInput.options[hourInput.value].text);
        SunPositionCalculate.Instance.minute = int.Parse(minuteInput.options[minuteInput.value].text);
        SunPositionCalculate.Instance.second = int.Parse(secondInput.options[secondInput.value].text);
    }

    public void UpdateOutputValue()
    {
        SunPositionCalculate.Instance.CalculateSunPosition();

        EotTxt.text = SunPositionCalculate.Instance.EoT.ToString("0.000");
        declinationAngleTxt.text = SunPositionCalculate.Instance.declinationAngle.ToString("0.000");
        sunriseTxt.text = SunPositionCalculate.Instance.sunrise.ToString("0.000");
        sunsetTxt.text = SunPositionCalculate.Instance.sunset.ToString("0.000");
        elevationAngleTxt.text = SunPositionCalculate.Instance.elevationAngle.ToString("0.000");
        azimuthAngleTxt.text = SunPositionCalculate.Instance.azimuthAngle.ToString("0.000");
    }
}
