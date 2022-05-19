using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CollectableType 
{
    healthPotion,
    manaPotion,
    money
}

public class Collectable : MonoBehaviour
{
    public CollectableType type = CollectableType.money;
    
    public int value = 1;

    private SpriteRenderer sprite;
    private CircleCollider2D itemCollider;

    bool hasBeenCollected = false;

    GameObject player;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Show()
    {
        sprite.enabled = true;
        itemCollider.enabled = true;
        hasBeenCollected = false;
    }

    void Hide()
    {
        sprite.enabled = false;
        itemCollider.enabled = false;
    }

    void Collect()
    {
        Hide();
        hasBeenCollected = true;

        switch(this.type)
        {
            case CollectableType.money:
                GameManager.sharedInstance.CollectObject(this);
                GetComponent<AudioSource>().Play();
                break;
            case CollectableType.healthPotion:
                player.GetComponent<PlayerController>().CollectHealth(this.value);
                break;
            case CollectableType.manaPotion:
                player.GetComponent<PlayerController>().CollectMana(this.value);
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Collect();
        }
    }
}
