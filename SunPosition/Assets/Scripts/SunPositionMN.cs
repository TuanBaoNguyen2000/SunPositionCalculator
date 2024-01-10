using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunPositionMN : MonoBehaviour
{
    private void Update()
    {
        SetSunPosition();
    }


    private void SetSunPosition()
    {
        Vector3 sunPos = new Vector3();
        sunPos.x = SunPositionCalculate.Instance.ElevationAngle();
        sunPos.y = SunPositionCalculate.Instance.AzimuthAngle();
        sunPos.z = 0;
        Quaternion sunRot = new Quaternion();
        sunRot = Quaternion.Euler(sunPos.x, sunPos.y, sunPos.z);    

        transform.rotation = sunRot;
    }
    
}
