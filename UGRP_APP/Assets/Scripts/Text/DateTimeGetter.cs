using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DateTimeGetter
{
    // Start is called before the first frame update
    public static string getNowString()
    {
        return System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
    }

}
