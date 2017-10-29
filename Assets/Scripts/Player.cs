using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

  // Physics
  private float playerSpeed;
  private float attack;
  private Rigidbody2D rb;
  //

  // Health, Items
  private float health;
  private float magic;
  //

  // Player State
  private bool idle;
  private bool attacking;
  private bool dead;
  //

  // Unity data
  private SpriteRenderer sr;
  private Sprite[] sprites;
  private string[] spriteNames;
  private Animator animator;

  // Other
  private const string PLAYER_ONE = "Player 1";
  private const string PLAYER_TWO = "Player 2";

  // Use this for initialization
  void Start () {
    rb = gameObject.GetComponent<Rigidbody2D>();
    animator = gameObject.GetComponent<Animator>();

    initPlayer();
  }

  void initPlayer() {
    // state
    dead = false;

    // settings
    playerSpeed = 1.0f;
    health = 3f;
    attack = 1f;
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
    Vector3 move;
    float horizontal = 0f;
    float vertical = 0f;
    float attack = 0f;

    if (this.gameObject.tag == PLAYER_ONE) {
      horizontal = Input.GetAxis("Horizontal") * playerSpeed;
      vertical = Input.GetAxis("Vertical") * playerSpeed;
      attack = Input.GetAxis("Fire1");
    } else if (this.gameObject.tag == PLAYER_TWO) {
      horizontal = Input.GetAxis("Horizontal2") * playerSpeed;
      vertical = Input.GetAxis("Vertical2") * playerSpeed;
      attack = Input.GetAxis("Fire2");
    }

    move = new Vector3(horizontal, vertical, 0);
    if (horizontal != 0f || vertical != 0f) {
      idle = false;
      attacking = false;
    } else if (attack != 0f) {
      idle = false;
      attacking = true;
    }
    else {
      idle = true;
      attacking = false;
    }

    rb.velocity = new Vector2(horizontal, vertical);

    UpdateSprite(move);
  }

  void UpdateSprite(Vector3 move) {
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
    Debug.Log("Collision...");
    if (coll.gameObject.tag == "Enemy" && attacking) {
      Debug.Log("Attack");
      // pass in gameobject of enemy
      Attack(coll);
    }
  }

  void Attack(Collision2D coll) {
    GameObject enemy = coll.gameObject;
    Debug.Log("Damaging enemy...");

    enemy.GetComponent<Player>().Damage(attack);
  }

  public void Damage(float dmg) {
    health -= dmg;
    // play damange animation

    Debug.Log("Health: " + health);
    if (health <= 0f) {
      // Trigger death animation
      dead = true;
      // game over
      // This Scene Manager call should prboably go
      // in the GameManager class once it's written
      SceneManager.LoadScene("GameOver");
    }
  }
}
