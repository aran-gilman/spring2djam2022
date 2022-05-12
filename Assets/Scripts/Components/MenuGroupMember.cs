using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGroupMember : MonoBehaviour
{
    public MenuGroup group;

    private void Awake()
    {
        group.RegisterMember(this);
    }
}
