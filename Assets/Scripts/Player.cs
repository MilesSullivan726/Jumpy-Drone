using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D rb;
    private float horizontalInput;
    private float lastAttack;
    private Vector3 shootPos;
    private bool hasJumped = false;
    private string direction;
    public GameObject projectile;
    public float speed = 1;
    public float jumpHeight;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        //movement
        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            Input.GetKeyUp(KeyCode.A);
        }

        //determine direction for projectile
        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.W))
            {
                direction = "UpLeft";
            }
            else
            {
                direction = "Left";
            }
           
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.W))
            {
                direction = "UpRight";
            }
            else
            {
                direction = "Right";
            }
           
        }
        else if (Input.GetKey(KeyCode.W))
        {
            direction = "Up";
           
        }
        

        //jump
        if (Input.GetKeyDown(KeyCode.Space) && !hasJumped)
        {
            
            hasJumped = true;
            //rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
        }

        //lock position for fine aim
        if (Input.GetKey(KeyCode.L) && !hasJumped)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        //shoot
        if (Input.GetKeyDown(KeyCode.M) && Time.time - lastAttack >= 0.3f)
        {

            lastAttack = Time.time;
            shootPos = transform.position;
            Shoot(direction, shootPos);
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(Vector2.right * horizontalInput * Time.deltaTime * speed, ForceMode2D.Impulse);
        
    }

    void Shoot(string direction, Vector3 shootPos)
    {
        if(direction == "Left")
        {
            Instantiate(projectile, shootPos, Quaternion.Euler(0, 0, 90));
        }
        else if (direction == "Right")
        {
            Instantiate(projectile, shootPos, Quaternion.Euler(0, 0, -90));
        }
        else if (direction == "Up")
        {
            Instantiate(projectile, shootPos, Quaternion.Euler(0, 0, 0));
        }
        else if (direction == "UpRight")
        {
            Instantiate(projectile, shootPos, Quaternion.Euler(0, 0, -45));
        }
        else if (direction == "UpLeft")
        {
            Instantiate(projectile, shootPos, Quaternion.Euler(0, 0, 45));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // prevent infinite jumps by enabling jump after touching ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            hasJumped = false;
            
        }
    }
}
