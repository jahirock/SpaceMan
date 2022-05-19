using System.Runtime.CompilerServices;
using System;
using System.Globalization;
using System.Runtime;
using System.Numerics;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 6f;
    public float disRay = 0.1F;
    public float runningSpeed = 2F;

    public LayerMask groundMask;

    Animator animator;

    Rigidbody2D rigidBody;

    CapsuleCollider2D collider;

    UnityEngine.Vector3 startPosition;
    UnityEngine.Vector2 startOffsetGround;

    const string STATE_ALIVE = "isAlive";
    const string STATE_ON_THE_GROUND = "isOnTheGround";
    const string STATE_FALLING = "isFalling";
    public const int INITIAL_HEALTH = 100, INITIAL_MANA = 15, MAX_HEALTH = 200, MAX_MANA = 30, MIN_HEALTH = 10, MIN_MANA = 0;

    public const int SUPERJUMP_COST = 5;
    public const float SUPERJUMP_FORCE = 1.5F;

    [SerializeField] //Para poder ver las variables dentro del editor de Unity
    private int healthPoints, manaPoints;


    void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
        startOffsetGround = this.collider.offset;
    }

    public void StartGame()
    {
        //Cambia los estados de animacion
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_THE_GROUND, false);
        animator.SetBool(STATE_FALLING, true);

        healthPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;

        //Ejecuta RestartPosition .2 segundos despues
        Invoke("RestartPosition", 0.2F);
    }

    void RestartPosition()
    {
        //Le quita la velocidad y reinicia la posicion del jugador
        this.rigidBody.velocity = UnityEngine.Vector2.zero;
        this.transform.position = startPosition;
        this.collider.offset = startOffsetGround;
        GameObject mainCamera = GameObject.Find("Main Camera");
        mainCamera.GetComponent<CameraFollow>().ResetCameraPosition();
    }

    // Update is called once per frame
    void Update()
    {
        //Se dibuja un rayo
        Debug.DrawRay(this.transform.position, UnityEngine.Vector2.down * (this.collider.size.y/2+disRay), Color.green);
        //Si se pulsa el espacio o el click izquierdo, Salta
        //if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        
        //No se especifica el boton, solo la accion. El InputMamager es el encargado de saber que boton le corresponde
        if(Input.GetButtonDown("Jump"))
        {
            Jump(false);
        }

        if(Input.GetButtonDown("SuperJump"))
        {
            Jump(true);
        }

        animator.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGround());
        animator.SetBool(STATE_FALLING, IsFalling());
    }

    void FixedUpdate()
    {
        //Si el juego esta iniciado
        if(GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if(rigidBody.velocity.x < runningSpeed) 
            {
                rigidBody.velocity = new UnityEngine.Vector2(runningSpeed, rigidBody.velocity.y);
            }
        }
        else 
        {
            //Si el juego no esta iniciado
            rigidBody.velocity = new UnityEngine.Vector2(0, rigidBody.velocity.y);
        }
    }

    void Jump(bool superJump)
    {
        float jumpForceFactor = jumpForce;

        if(superJump && manaPoints >= SUPERJUMP_COST)
        {
            manaPoints -= SUPERJUMP_COST;
            jumpForceFactor *= SUPERJUMP_FORCE;
        }

        if(GameManager.sharedInstance.currentGameState == GameState.inGame && IsTouchingTheGround())
        {
            GetComponent<AudioSource>().Play();
            //Le añade una fuerza hacia arriba con la fuerza de salto
            rigidBody.AddForce(UnityEngine.Vector2.up * jumpForceFactor, ForceMode2D.Impulse);
        }
    }

    bool IsTouchingTheGround()
    {
        //Se lanza un rayo desde la posicion del player, hacia abajo, a una distancia de la mitad del tamaño del collider + un poco, hasta la mascara del suelo
        return Physics2D.Raycast(this.transform.position, UnityEngine.Vector2.down, this.collider.size.y/2+disRay, groundMask);
    }

    bool IsFalling(){
        return rigidBody.velocity.y < 0;
    }

    public void Die()
    {
        float travelledDistance = GetTravelledDistance();
        float previousMaxDistance = PlayerPrefs.GetFloat("maxscore", 0F);

        if(travelledDistance > previousMaxDistance)
        {
            PlayerPrefs.SetFloat("maxscore", travelledDistance);
        }

        //Para que al morir el personaje no quede flotando
        collider.offset = new UnityEngine.Vector2(0,0);

        this.animator.SetBool(STATE_ALIVE, false);
        GameManager.sharedInstance.GameOver();
    }

    public void CollectHealth(int points)
    {
        this.healthPoints += points;
        if(this.healthPoints > MAX_HEALTH)
        {
            this.healthPoints = MAX_HEALTH;
        }
        if(this.healthPoints <= 0)
        {
            Die();
        }
    }

    public void CollectMana(int points)
    {
        this.manaPoints += points;
        if(this.manaPoints > MAX_MANA)
        {
            this.manaPoints = MAX_MANA;
        }
    }

    public int GetHealth()
    {
        return healthPoints;
    }

    public int GetMana()
    {
        return manaPoints;
    }

    public float GetTravelledDistance()
    {
        return this.transform.position.x - startPosition.x;
    }
}
