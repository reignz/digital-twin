using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHelper
{
    // Start is called before the first frame update

    public string ZoneTranslation(int number)
    {
        switch (number)
        {
            case 0:
                return "Зеленая";
            case 1:
                return "Желтая";
            case 2:
                return "Красная";
            default:
                return "default";
        }
    }
}
