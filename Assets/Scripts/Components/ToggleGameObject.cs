using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        if (gameObject.activeSelf)
        {
            audioSource.PlayOneShot(openSound);
        }
        else
        {
            audioSource.PlayOneShot(closeSound);
        }
    }
}
