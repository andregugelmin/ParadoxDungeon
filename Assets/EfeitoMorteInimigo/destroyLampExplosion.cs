using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyLampExplosion : MonoBehaviour
{
    public float decrease;
    private Light lamp;
    
    void Start()
    {
        lamp = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        lamp.range += decrease * Time.deltaTime;
        if (lamp.range >= 8.24f)
            Destroy(gameObject);
    }
}
