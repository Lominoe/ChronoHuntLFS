using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public Player player;
    public BoxCollider2D box;
    public HitStop hitStop;
    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.tag == "Player")
        {
            hitStop.Stop(.5f);
            player.DamagePlayer(5f);
        }
        
    }
}
