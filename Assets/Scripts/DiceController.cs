using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    // The amount of time this die has been "alive"
    private float timeAlive;
    // The die's lifespan. When this amount of time has passed, delete the die.
    private float lifespan;

    // Start is called before the first frame update
    void Awake()
    {
        timeAlive = 0;
        lifespan = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive > lifespan)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
