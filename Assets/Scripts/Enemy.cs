using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float runningSpeed = 1.5F;
    public bool facingRight = false;

    public int enemyDamage = 10;

    UnityEngine.Vector3 startPosition;

    Rigidbody2D rigidBody;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        //this.transform.position = startPosition;
    }
    
    void FixedUpdate()
    {
        float currentRunningSpeed = runningSpeed;
        if(facingRight)
        {
            //Hacia la derecha
            currentRunningSpeed = runningSpeed;
            this.transform.eulerAngles = new UnityEngine.Vector3(0, 180, 0);
        }
        else 
        {
            //Hacia la izquierda
            currentRunningSpeed = -runningSpeed;
            this.transform.eulerAngles = UnityEngine.Vector3.zero;
        }

        if(GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            rigidBody.velocity = new UnityEngine.Vector2(currentRunningSpeed, rigidBody.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().CollectHealth(-enemyDamage);
        }

        if(other.tag == "collision")
        {
            facingRight = !facingRight;
        }

    }
}
