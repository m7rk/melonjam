using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APPSTATE : MonoBehaviour
{
    // -1 -> disable tutorial.
    public static int TUTORIAL_STAGE = 0;
    public static int LEVEL = 0;

    public static int GRENADE_COUNT = 2;
    public static int CLEAR_COUNT = 2;
    public static int FILL_COUNT = 2;

    public static void reset()
    {
        // don't re run the tutorial ever
        LEVEL = 0;
        GRENADE_COUNT = 2;
        CLEAR_COUNT = 2;
        FILL_COUNT = 2;
    }


    public static int getBPMForLevel()
    {
        switch (APPSTATE.LEVEL)
        {
            case 0:
                return 80;
            case 1:
                return 100;
            case 2:
                return 120;
        }
        Debug.Log("level OOB");
        return 80;
    }
}
