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

  // Use this for initialization
  void Start () {
    playerSpeed = 1.0f;
    health = 3f;
    rb = gameObject.GetComponent<Rigidbody2D>();

    sr = gameObject.GetComponent<SpriteRenderer>();
    sprites = Resources.LoadAll<Sprite>("Sprites");

    spriteNames = new string[sprites.Length];
    for (int i=0; i < sprites.Length; i++) {
      spriteNames[i] = sprites[i].name;
    }
    Debug.Log("Loaded sprites: " + sprites.Length);
  }

  // Update is called once per frame
  void Update () {
    Move();
  }

  void Move() {
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");

    var move = new Vector3(horizontal, vertical, 0);
    if (horizontal != 0f || vertical != 0f) {
      Debug.Log("Moving Link..");
      Debug.Log(move);
    }

    UpdateSprite(move);
    transform.position += move * playerSpeed * Time.deltaTime;
  }

  int GetSpriteIndex(string name) {
    return Array.IndexOf(spriteNames, name);
  }

  void UpdateSprite(Vector3 move) {
    int s;

    if (move.y > 0) {
      s = GetSpriteIndex("link-backward_4");
      sr.sprite = sprites[s];
    } else if (move.y < 0) {
      s = GetSpriteIndex("link-forward_4");
      sr.sprite = sprites[s];
    }

    if (move.x > 0) {
      s = GetSpriteIndex("link-sideways_3");
      sr.sprite = sprites[s];
      sr.flipX = false;
    } else if (move.x < 0) {
      s = GetSpriteIndex("link-sideways_3");
      sr.sprite = sprites[s];
      sr.flipX = true;
    }
  }
}
