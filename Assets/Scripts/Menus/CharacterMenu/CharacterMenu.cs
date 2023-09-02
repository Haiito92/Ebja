using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{




    //variables pour un test
    public Image backgroundMenu;
    public Sprite[] backgroundSprites;

    public CharacterData Tenia;
    public CharacterData Onarys;

    public DataPart dataPart;

    private void Start()
    {
        LoadMenu(Onarys);
    }

    public void LoadMenu(CharacterData chardata)
    {
        dataPart.LoadDataPart(chardata);
        backgroundMenu.sprite = backgroundSprites[(int)chardata.tribe];

    }
}
