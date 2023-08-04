using UnityEngine;

public class PlayerMoment : MonoBehaviour
{
    private new Camera camera;
    private new Rigidbody2D rigidbody;
    // private new Rigidbody2D rigidbody;

    public float movSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    public float JumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow((maxJumpTime / 2f), 2);

    public bool grounded { get; private set; }
    public bool jumping { get; private set;}
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f; 
    public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f); 
    

    private Vector2 velocity;
    private float inputAxis;

    private void Awake(){
        rigidbody = GetComponent<Rigidbody2D>();
        camera = Camera.main;
    }

    private void Update(){

        HorizontalMovement();
        grounded = rigidbody.Raycast(Vector2.down);

        if (grounded){
            GroundedMovement();
        }
        
        ApplyGravity();
    }

    private void HorizontalMovement(){
        inputAxis = Input.GetAxis("Horizontal");
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * movSpeed, movSpeed * Time.deltaTime);

        //if(rigidbody.Raycast(Vector2.right * velocity.x)){
        //  velocity.x = 0f; 
        //}

        if(velocity.x > 0f){
            transform.eulerAngles = Vector3.zero;
        }
        else if(velocity.x < 0f){
            transform.eulerAngles = new Vector3(0f, 180f, 0f);            
        }
    }

    private void GroundedMovement(){
        // prevent gravity from infinitly building up
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;

        // perform jump
        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = JumpForce;
            jumping = true;
        }
    }

    private void ApplyGravity(){
        // check if falling
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2.5f : 1f;

        // apply gravity and terminal velocity
        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    private void FixedUpdate(){
        Vector2 position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;

        Vector2 leftedge = camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightedge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftedge.x + 0.5f, rightedge.x - 0.5f);

        rigidbody.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.layer != LayerMask.NameToLayer("PowerUp")){
            //velocity.y = 0f;
            if(transform.DotTest(collision.transform, Vector2.up)){
                velocity.y = 0f;
            }
        }

    }

}