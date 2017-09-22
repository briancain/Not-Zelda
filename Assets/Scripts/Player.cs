using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour {

  // Physics
  private float playerSpeed;
  private Rigidbody2D rb;
  //

  // Health, Items
  private float health;
  //

  // Unity data
  private SpriteRenderer sr;
  private Sprite[] sprites;
  private string[] spriteNames;
  private Animator animator;

  // Use this for initialization
  void Start () {
    playerSpeed = 1.0f;
    health = 3f;
    rb = gameObject.GetComponent<Rigidbody2D>();
    animator = gameObject.GetComponent<Animator>();

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
    Move();
  }

  void Move() {
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");
    bool idle;

    var move = new Vector3(horizontal, vertical, 0);
    if (horizontal != 0f || vertical != 0f) {
      idle = false;
      Debug.Log("Moving Link..");
      Debug.Log(move);
      transform.position += move * playerSpeed * Time.deltaTime;
    } else {
      idle = true;
    }

    UpdateSprite(move, idle);
  }

  int GetSpriteIndex(string name) {
    return Array.IndexOf(spriteNames, name);
  }

  void UpdateSprite(Vector3 move, bool idle) {
    animator.SetBool("Idle", idle);

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

    if (!idle) {
      Debug.Log("Animators Idle: " + animator.GetBool("Idle"));
      Debug.Log("Animators Direction: " + animator.GetInteger("Direction"));
    }

  }
}
