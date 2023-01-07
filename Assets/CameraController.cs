using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float followSpeed = 0.05f;
    
    void LateUpdate()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        if (!player) return;

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 Playerpos = new Vector2(player.transform.position.x, player.transform.position.y);
        pos = Vector2.Lerp(pos, Playerpos, followSpeed);
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }
}
