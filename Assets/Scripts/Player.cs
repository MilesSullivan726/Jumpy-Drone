using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D rb;
    private Rigidbody2D platformRB;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    public Sprite neutral;
    public Sprite diagonal;
    public Sprite upward;
    private float horizontalInput;
    private float lastAttack;
    private Vector3 shootPos;
    private Vector2 platformVelocity;
    public bool hasJumped = false;
    private string direction;
    public GameObject projectile;
    public AudioClip jumpSFX;
    public AudioClip shootSFX;
    public float speed = 1;
    public float jumpHeight;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        

        horizontalInput = Input.GetAxisRaw("Horizontal");

        

        //determine direction for projectile
        if (Input.GetKey(KeyCode.A))
        {
            spriteRenderer.flipX = true;
            if (Input.GetKey(KeyCode.W))
            {
                direction = "UpLeft";
                spriteRenderer.sprite = diagonal;
            }
            else
            {
                direction = "Left";
                spriteRenderer.sprite = neutral;
            }
           
        }
        else if (Input.GetKey(KeyCode.D))
        {
            spriteRenderer.flipX = false;
            if (Input.GetKey(KeyCode.W))
            {
                direction = "UpRight";
                spriteRenderer.sprite = diagonal;
            }
            else
            {
                direction = "Right";
                spriteRenderer.sprite = neutral;
            }
           
        }
        else if (Input.GetKey(KeyCode.W))
        {
            direction = "Up";
            spriteRenderer.sprite = upward;
        }
        

        //jump
        if (Input.GetKeyDown(KeyCode.Space) && !hasJumped)
        {
            audioSource.volume = 0.5f;
            audioSource.PlayOneShot(jumpSFX);
            hasJumped = true;
            //rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
        }

        //lock position for fine aim
        if (Input.GetKey(KeyCode.L))
        {
            rb.constraints = ~RigidbodyConstraints2D.FreezePositionY;

        }
        else
        {
            rb.constraints = ~RigidbodyConstraints2D.FreezePositionX & ~RigidbodyConstraints2D.FreezePositionY;
            //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        //shoot
        if (Input.GetKeyDown(KeyCode.M) && Time.time - lastAttack >= 0.3f)
        {
            audioSource.volume = 1;
            audioSource.PlayOneShot(shootSFX);
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
            if(rb.velocity.x > 0)
            {
               shootPos = shootPos + new Vector3(1.35f, 1, 0);
            }
            Instantiate(projectile, shootPos, Quaternion.Euler(0, 0, -45));
        }
        else if (direction == "UpLeft")
        {
            if (rb.velocity.x < 0)
            {
                shootPos = shootPos + new Vector3(-1.35f, 1, 0);
            }
            Instantiate(projectile, shootPos, Quaternion.Euler(0, 0, 45));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // prevent infinite jumps by enabling jump after touching ground
        /*
        if (collision.gameObject.CompareTag("PlatformMove"))
        {
           // Debug.Log("On Platform");
            hasJumped = false;
            //gameObject.transform.parent = collision.transform.parent;
            
            // VVV put this in update or void update VVV
            //platformRB = collision.transform.parent.GetComponent<Rigidbody2D>();
            
        }

        else if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            hasJumped = false;
            
        }
        */
        

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlatformMove"))
        {

            gameObject.transform.parent = null;
        }
    }





}
