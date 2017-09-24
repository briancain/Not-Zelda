using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour {

  // Physics
  private float playerSpeed;
  private float damage;
  private Rigidbody2D rb;
  //

  // Health, Items
  private float health;
  private float magic;
  //

  // Unity data
  private SpriteRenderer sr;
  private Sprite[] sprites;
  private string[] spriteNames;
  private Animator animator;

  // Use this for initialization
  void Start () {
    rb = gameObject.GetComponent<Rigidbody2D>();
    animator = gameObject.GetComponent<Animator>();

    initPlayer();
  }

  void initPlayer() {
    playerSpeed = 1.0f;
    health = 3f;
    damage = 1f;
    magic = 0f;
  }

  int GetSpriteIndex(string name) {
    return Array.IndexOf(spriteNames, name);
  }

  void LoadSprites() {
    sr = gameObject.GetComponent<SpriteRenderer>();
    sprites = Resources.LoadAll<Sprite>("Sprites/Link");

    spriteNames = new string[sprites.Length];
    for (int i=0; i < sprites.Length; i++) {
      spriteNames[i] = sprites[i].name;
    }
    Debug.Log("Loaded Link sprites: " + sprites.Length);
  }

  // Update is called once per frame
  void Update () {
    float horizontal = Input.GetAxis("Horizontal") * playerSpeed;
    float vertical = Input.GetAxis("Vertical") * playerSpeed;
    float attack = Input.GetAxis("Fire1");
    bool idle, attacking;

    var move = new Vector3(horizontal, vertical, 0);
    if (horizontal != 0f || vertical != 0f) {
      idle = false;
      attacking = false;
      //transform.position += move * playerSpeed * Time.deltaTime;
    } else if (attack != 0f) {
      idle = false;
      attacking = true;
    }
    else {
      idle = true;
      attacking = false;
    }

    rb.velocity = new Vector2(horizontal, vertical);

    UpdateSprite(move, idle, attacking);
  }

  void UpdateSprite(Vector3 move, bool idle, bool attacking) {
    animator.SetBool("Idle", idle);
    animator.SetBool("Attacking", attacking);

    if (move.y > 0) {
      // backwards
      animator.SetInteger("Direction", 1);
    }
    if (move.y < 0) {
      // forward
      animator.SetInteger("Direction", 2);
    }
    if (move.x > 0) {
      // sideways right
      animator.SetInteger("Direction", 3);
    }
    if (move.x < 0) {
      // sideways left
      animator.SetInteger("Direction", 4);
    }
  }

  void OnCollisionEnter2D(Collision2D coll) {
  }
}
