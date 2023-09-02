using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitSlot : MonoBehaviour
{
    private CharacterMenu characterMenu;

    public CharacterData chardata;

    private void Start()
    {
        characterMenu = GameObject.Find("Character Menu").GetComponent<CharacterMenu>();
    }

    public void SendData()
    {
        characterMenu.LoadMenu(chardata);
    }
    
}
