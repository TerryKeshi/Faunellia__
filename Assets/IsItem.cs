using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IsItem : MonoBehaviour
{
    public string name;
    public int count;
    public bool throwable;
    public Sprite image;
    public string pickUpAudioName;
    public bool _protected;
    public bool _locked;

    [Header("Throwing:")]

    public int rotationX;
    public int rotationY;
    public int rotationZ;    

    [Header("Do not set:")]

    public Rigidbody rb;
    public Collider collider;
    public Renderer _renderer;
    public Transform _transform;
    public GameObject obj;

    AllFather _allFather;
    string _id;

    public void Start()
    {
        _allFather = GameObject.Find("AllFather").GetComponent<AllFather>();
        _id = "" + transform.position.x + transform.position.y + transform.position.z;

        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        _renderer = GetComponent<Renderer>();
        _transform = GetComponent<Transform>();
        obj = gameObject;

/*        Save s = new Save();

        if (_allFather.Contains(_id))
        {
            s = _allFather.Load(_id);

            _locked = s._locked;

            if (s._hidden)
                Hide();
            if (s._destroyed)
                Destroy();
        }
        else
        {
            s = new Save();
            s._position = _transform.position;
            s._rotation = _transform.rotation;
            _allFather.Save(_id, s);
        }*/
    }

    public void ToggleLock(bool locked)
    {
        Save s = new Save();
        if (_allFather.Contains(_id))
            s = _allFather.Load(_id);

        _locked = locked;
        s._locked = locked;

        _allFather.Save(_id, s);
    }

    public void Hide()
    {
        if (!_protected)
        {
            if (rb != null)
                rb.isKinematic = true;

            _renderer.enabled = false;
            collider.enabled = false;

            Save es = _allFather.Load(_id);
            es._hidden = true;            
            _allFather.Save(_id, es);

            foreach (Transform child in transform)
                Destroy(child.gameObject);
        }
    }

    public void Destroy()
    {
        if (!_protected)
        {
            Save save = _allFather.Load(_id);
            save._destroyed = true;
            _allFather.Save(_id, save);

            foreach (Transform child in transform)
                Destroy(child.gameObject);

            Destroy(gameObject);
        }
    }

    public void Throw(Vector3 position, Vector3 direction, float power, Vector3 playerVelocity, Quaternion rotation)
    {
        transform.position = position + direction;
        transform.rotation = rotation * Quaternion.Euler(rotationX, rotationY, rotationZ);
        _renderer.enabled = true;
        collider.enabled = true;

        if (rb != null)
            rb.isKinematic = false;

        rb.velocity = playerVelocity;
        rb.AddForce(direction * power * rb.mass);
    }

    public IsItem Clone()
    {
        GameObject clone = Instantiate(obj);
        Transform ct = clone.GetComponent<Transform>();
        ct.position += new Vector3(1, 1, 1);
        IsItem isItem = clone.GetComponent<IsItem>();
        isItem._id = "" + ct.position.x + ct.position.y + ct.position.z;
        Save save = _allFather.Load(_id) as Save;
        if (save == null)
            save = new Save();
        save._destroyed = false;
        _allFather.Save(isItem._id, save);        
        return isItem;
    }
}
