using SwkGDCShortCourse.Player;
using UnityEngine;

namespace SwkGDCShortCourse.Level
{
    [RequireComponent(typeof(Collider2D))]
    public class LevelDivider : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag.Equals("Player") && collision.TryGetComponent(out PlayerController playerController))
            {
                GameManager.Instance.InvokeOnChangedLevel(playerController);
            }
        }
    }
}
