using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTaker : MonoBehaviour
{
    public Inventory _inventory;
    public IsItem _item1;
    public IsItem _item2;

    void Start()
    {
        StartCoroutine(Start0());
    }

    IEnumerator Start0()
    {
        yield return new WaitForSeconds(0.5f);

        _inventory.Take(_item1);

        yield return new WaitForSeconds(0.1f);

        _inventory.Take(_item2);
    }
}
