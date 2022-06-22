using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingDetector : MonoBehaviour
{
    [SerializeField]
    private RotatingBox _rotating;

    private Player player;

    private void Start()
    {
        player = GameManager.Instance.Player.GetComponent<Player>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject == player.gameObject) return;
        if (_rotating.RotateValue == 0)
        {
            player.RotateValue = 0;
        }
        else if (_rotating.RotateValue == 1)
        {
            player.RotateValue = -1;
        }
        else if (_rotating.RotateValue == -1)
        {
            player.RotateValue = 1;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject == player.gameObject) return;
        if (_rotating.RotateValue == 0)
        {
            player.RotateValue = 0;
        }
        else if (_rotating.RotateValue == 1)
        {
            player.RotateValue = -1;
        }
        else if (_rotating.RotateValue == -1)
        {
            player.RotateValue = 1;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject == player.gameObject) return;
        player.RotateValue = 0;
    }
}
