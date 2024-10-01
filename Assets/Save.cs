using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public Vector3 _position;
    public Quaternion _rotation;
    public float _health;
    public bool _hidden;
    public bool _destroyed;
    public bool _pressed;
    public bool _opened;
    public bool _locked;
    public int _count;
}
