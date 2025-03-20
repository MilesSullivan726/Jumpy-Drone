using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
   
    private GameObject childCollider;
    // Start is called before the first frame update
    void Start()
    {
        

        childCollider = transform.Find("Child").gameObject;
       // GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics2D.IgnoreCollision(childCollider.GetComponent<Collider2D>(), gameObject.GetComponent<BoxCollider2D>());
    }

    // Update is called once per frame
    private void Update()
    {
       
    }


}
