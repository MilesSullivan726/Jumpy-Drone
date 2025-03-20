using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private Transform top;
    private Rigidbody2D topRB;
    private Rigidbody2D platformRB;
    private GameObject platform;
    private AudioSource audioSource;
    private bool canBePressed = true;
    private bool reverse = true;
    private bool oneWayDone = false;
    private float platformWidth;
    private float distanceMoved;
    private float distToMove;
    private int platformsDone = 0;
    private Vector3 startPos;
    
    public GameObject[] platforms;
    public AudioClip downSFX;
    public AudioClip upSFX;
    public float moveDistance = 2;
    public bool isOneWay = false;
    
    // Start is called before the first frame update
    void Start()
    {
        top = transform.Find("Top");
        topRB = top.GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator ButtonAction(Rigidbody2D platformRB, Vector3 startPos, GameObject platform)
    {
        if (!isOneWay || !oneWayDone)
        {



            canBePressed = false;

            // for(int i = 0; i < platforms.Length; i++)
            // {



            //platformRB = platforms[i].GetComponent<Rigidbody2D>();
            //startPos = platforms[i].transform.localPosition;
            if (!reverse)
            {
                platformRB.AddRelativeForce(Vector2.up, ForceMode2D.Impulse);
            }
            else
            {
                platformRB.AddRelativeForce(Vector2.down, ForceMode2D.Impulse);
            }
            distanceMoved = 0;

            if (platform.transform.localRotation.eulerAngles.z == 90 || platform.transform.localRotation.eulerAngles.z == 270)
            {

                platformWidth = platform.GetComponent<BoxCollider2D>().bounds.extents.x * moveDistance;

                float targetPositionX = startPos.x + platformWidth;

                while (Mathf.Abs(platform.transform.localPosition.x - startPos.x) < platformWidth)
                {

                    float moveAmount = 0.1f * Time.deltaTime;



                    platform.transform.localPosition += new Vector3(moveAmount, 0, 0);

                    yield return null;
                }
            }
            else
            {
                platformWidth = platform.transform.GetComponent<BoxCollider2D>().bounds.extents.y * moveDistance;

                float targetPositionY = startPos.y + platformWidth;

                while (Mathf.Abs(platform.transform.localPosition.y - startPos.y) < platformWidth)
                {

                    float moveAmount = 0.1f * Time.deltaTime;


                    platform.transform.localPosition += new Vector3(0, moveAmount, 0);

                    yield return null;
                }
            }



            //platformRB.AddRelativeForce(Vector2.up, ForceMode2D.Impulse);
            //platforms[i].transform.Translate(Vector3.up);
            //distanceMoved += startPos.y - platforms[i].transform.position.y;


            platformRB.velocity = Vector2.zero;
            //}

            platformsDone += 1;
            if (platformsDone == platforms.Length && !isOneWay)
            {
                platformsDone = 0;
                StartCoroutine(ButtonRaise());
            }

            oneWayDone = true;
        }
    }

    IEnumerator ButtonLower()
    {
        audioSource.PlayOneShot(downSFX);
        topRB.AddRelativeForce(Vector2.down, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.3f);
        topRB.velocity = Vector2.zero;
        
    }

    IEnumerator ButtonRaise()
    {
        audioSource.PlayOneShot(upSFX);
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
            
                for (int i = 0; i < platforms.Length; i++)
                {
                    platformRB = platforms[i].GetComponent<Rigidbody2D>();
                    startPos = platforms[i].transform.localPosition;
                    platform = platforms[i];
                    StartCoroutine(ButtonAction(platformRB, startPos, platform));
                }
            
        }
    }
}
