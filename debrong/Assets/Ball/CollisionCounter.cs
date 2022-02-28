using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCounter : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        STATIC_BounceCounter.Increment();
    }
}
