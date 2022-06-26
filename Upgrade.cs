using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public int id;
    public string afterCurrent;
    public GameObject panel;
    public int current;
    public int price;
    public int basePrice;
}
