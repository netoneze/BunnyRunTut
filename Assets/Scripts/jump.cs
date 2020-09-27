using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class jump : MonoBehaviour {

    private Rigidbody2D myRigidBody;
    private Animator myAnim;
    public float bunnyJumpForce = 500f;
    private float bunnyHurtTime = -1;
    private Collider2D myCollider;
    public Text scoreText;
    private float startTime;
    private int jumpsLeft = 2;
    public AudioSource jumping;
    public AudioSource dead;
    public AudioSource background;
	
    // Use this for initialization
	void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();

        startTime = Time.time;
        background.Play();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("Title");
        }


        if (bunnyHurtTime == -1)
        {

            if (Input.GetButtonUp("Jump") && jumpsLeft > 0)
            {
                if(myRigidBody.velocity.y < 0)
                {
                    myRigidBody.velocity = Vector2.zero;
                }

                if (jumpsLeft == 1)
                {
                    myRigidBody.AddForce(transform.up * bunnyJumpForce * 0.75f);
                }

                else
                {
                    myRigidBody.AddForce(transform.up * bunnyJumpForce);
                }


                jumpsLeft--;
                jumping.Play();
            }

            myAnim.SetFloat("vVelocity", myRigidBody.velocity.y);
            scoreText.text = (Time.time - startTime).ToString("0.0");

        }
        else
        {
            if (Time.time > bunnyHurtTime + 2)
            {
                SceneManager.LoadScene("Game");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            foreach (PreFabSpawner spawner in FindObjectsOfType<PreFabSpawner>())
            {
                spawner.enabled = false;
            }

            foreach (MoveLeft moveLefter in FindObjectsOfType<MoveLeft>())
            {
                moveLefter.enabled = false;
            }

            bunnyHurtTime = Time.time;
            myAnim.SetBool("bunnyhurt", true);
            myRigidBody.velocity = Vector2.zero;
            myRigidBody.AddForce(transform.up * bunnyJumpForce);
            myCollider.enabled = false;
            background.Stop();
            dead.Play();
            
        }

        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            jumpsLeft = 2;
        }

    }

}
