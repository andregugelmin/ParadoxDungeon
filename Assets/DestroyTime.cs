using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTime : MonoBehaviour
{
    public float destroyTime = 3f;
    public Vector3 Offset = new Vector3(0, 2, 0);
    public Quaternion rotation;
    void Start()
    {
        Destroy(gameObject, destroyTime);
        transform.localPosition += Offset;
        rotation = gameObject.transform.rotation;
    }
    private void Update()
    {
        transform.rotation = rotation;
    }


}
