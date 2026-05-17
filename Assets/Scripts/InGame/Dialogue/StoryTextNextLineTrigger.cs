using UnityEngine;

public class StoryTextNextLineTrigger : MonoBehaviour
{
    [SerializeField] private bool canDestroy = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || StoryTextManager.Instance == null)
            return;

        StoryTextManager.Instance.ShowNextLine();

        if (canDestroy)
        {
            Destroy(gameObject);
        }
    }
}
