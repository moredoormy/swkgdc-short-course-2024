using System;
using UnityEngine;

namespace SwkGDCShortCourse.Point
{
    [RequireComponent(typeof(Collider2D))]
    public class SpawnPoint : MonoBehaviour
    {
        private BoxCollider2D _collider;

        private void Awake() => _collider = GetComponent<BoxCollider2D>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag.Equals("Player"))
            {
                Vector2 spawnPoint = new(transform.position.x + 0.5f, transform.position.y);
                GameManager.Instance.SaveSpawnPoint(spawnPoint);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag.Equals("Player")) _collider.enabled = false; 
        }
    }
}
