using System.Collections.Generic;
using UnityEngine;

public class MenuGroup : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip menuOpenSound;
    public AudioClip menuCloseSound;

    public void ToggleMenu(MenuGroupMember menu)
    {
        foreach (MenuGroupMember otherMenu in members)
        {
            if (menu == otherMenu)
            {
                menu.gameObject.SetActive(!menu.gameObject.activeSelf);
                audioSource.PlayOneShot(menu.gameObject.activeSelf ? menuOpenSound : menuCloseSound);
            }
            else
            {
                otherMenu.gameObject.SetActive(false);
            }
        }    
    }

    public void RegisterMember(MenuGroupMember member)
    {
        if (members.Contains(member))
        {
            Debug.LogError($"{member.name} already registered to {name}");
            return;
        }
        members.Add(member);
    }

    private List<MenuGroupMember> members = new List<MenuGroupMember>();

    private void Start()
    {
        foreach (MenuGroupMember member in members)
        {
            member.gameObject.SetActive(false);
        }
    }
}
