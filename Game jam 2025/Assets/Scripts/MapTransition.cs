using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MapTransition : MonoBehaviour
{

    [SerializeField] PolygonCollider2D mapBoundary;
    CinemachineConfiner confiner;
    [SerializeField] Direction direction;
    [SerializeField] float additiviePos = 2;

    enum Direction { Up, Down, Left, Right }

    private void Awake()
    {
       confiner = FindObjectOfType<CinemachineConfiner>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            confiner.m_BoundingShape2D = mapBoundary;
            UpdatePlayerPosition(collision.gameObject);
        }
    }

    private void UpdatePlayerPosition(GameObject player)
    {
       Vector3 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                newPos.y += additiviePos;
                break;
            case Direction.Down:
                newPos.y -= additiviePos;
                break;
            case Direction.Left:
                newPos.x += additiviePos;
                break;
            case Direction.Right:
                newPos.x -= additiviePos;
                break;

        }

        player.transform.position = newPos;
    }
}
