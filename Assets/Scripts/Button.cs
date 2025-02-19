using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private Transform top;
    private Rigidbody2D topRB;
    private Rigidbody2D platformRB;
    private bool canBePressed = true;
    private bool reverse = true;
    private float platformWidth;
    private float distanceMoved;
    private float distToMove;
    private Vector3 startPos;
    
    public GameObject[] platforms;
    
    // Start is called before the first frame update
    void Start()
    {
        top = transform.Find("Top");
        topRB = top.GetComponent<Rigidbody2D>();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ButtonAction()
    {

        Debug.Log("function called");
        canBePressed = false;

        for(int i = 0; i < platforms.Length; i++)
        {
            Debug.Log("for loop");
           
            
            platformRB = platforms[i].GetComponent<Rigidbody2D>();
            startPos = platforms[i].transform.localPosition;
            if (!reverse)
            {
                platformRB.AddRelativeForce(Vector2.up, ForceMode2D.Impulse);
            }
            else
            {
                platformRB.AddRelativeForce(Vector2.down, ForceMode2D.Impulse);
            }
            distanceMoved = 0;
           
            if (platforms[i].transform.localRotation.eulerAngles.z == 90 || platforms[i].transform.localRotation.eulerAngles.z == 270)
            {
                
                platformWidth = platforms[i].GetComponent<BoxCollider2D>().bounds.extents.x * 2;

                float targetPositionX = startPos.x + platformWidth;
                
                while (Mathf.Abs(platforms[i].transform.localPosition.x - startPos.x) < platformWidth)
                {
                   
                    float moveAmount = 0.1f * Time.deltaTime;
                    

                    
                    platforms[i].transform.localPosition += new Vector3(moveAmount, 0, 0);

                    yield return null;
                }
            }
            else
            {
                platformWidth = platforms[i].GetComponent<BoxCollider2D>().bounds.extents.y * 2;

                float targetPositionY = startPos.y + platformWidth;

                while (Mathf.Abs(platforms[i].transform.localPosition.y - startPos.y) < platformWidth)
                {

                    float moveAmount = 0.1f * Time.deltaTime;
                    

                    platforms[i].transform.localPosition += new Vector3(0, moveAmount, 0);

                    yield return null;
                }
            }
            
            
                
                //platformRB.AddRelativeForce(Vector2.up, ForceMode2D.Impulse);
                //platforms[i].transform.Translate(Vector3.up);
                //distanceMoved += startPos.y - platforms[i].transform.position.y;

            
            platformRB.velocity = Vector2.zero;
        }
        
        
        StartCoroutine(ButtonRaise());
    }

    IEnumerator ButtonLower()
    {
        
        topRB.AddRelativeForce(Vector2.down, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.3f);
        topRB.velocity = Vector2.zero;
        
    }

    IEnumerator ButtonRaise()
    {

        topRB.AddRelativeForce(Vector2.up, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.3f);
        topRB.velocity = Vector2.zero;
        canBePressed = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile") && canBePressed)
        {
            reverse = !reverse;
            Debug.Log(reverse);
            StartCoroutine(ButtonLower());
            StartCoroutine(ButtonAction());
        }
    }
}
