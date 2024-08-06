using UnityEngine;

namespace SwkGDCShortCourse.Point
{
    [RequireComponent(typeof(Collider2D))]
    public class WinPoint : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag.Equals("Player"))
            {
                GameManager.Instance.SetWin();
            }
        }
    }
}
