using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ForestTree>() is ForestTree tree)
        {
            tree.OnConsumedByLava();
        }
        else if (collision.gameObject.GetComponentInParent<PlayerController>() is PlayerController player)
        {
            player.OnHitByBoulder();
        }
    }
}
