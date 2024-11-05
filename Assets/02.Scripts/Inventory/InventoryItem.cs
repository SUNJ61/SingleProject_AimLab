using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public GameObject Item;

    public InventoryItem (GameObject item)
    {
        Item = item;
    }
}
