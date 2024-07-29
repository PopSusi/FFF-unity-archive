using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteBased : Damageable
{
    SpriteRenderer spritey;
    PlayerController player;
    // Start is called before the first frame update
    protected void Start()
    {
        player = PlayerController.instance;
    }

    // Update is called once per frame
    protected void Rotate()
    {
        transform.LookAt(player.transform.position, Vector3.up);
    }
}
