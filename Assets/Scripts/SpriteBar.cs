using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteBar : MonoBehaviour
{
    // later do a slick animation
    public void set(float amt)
    {
        transform.Find("Fill").GetComponent<Image>().fillAmount = amt;
    }
}
