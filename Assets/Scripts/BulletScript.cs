using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private void Awake()
    {
        Invoke("KillYourself", 10f);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void KillYourself()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        KillYourself();
    }
}
