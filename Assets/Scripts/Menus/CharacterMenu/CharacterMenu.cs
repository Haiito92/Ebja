using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMenu : MonoBehaviour
{

    
    
    
    //variables pour un test
    
    public CharacterData Tenia;
    public CharacterData Onarys;

    public DataPart dataPart;

    private void Start()
    {
        dataPart.LoadDataPart(Tenia);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            dataPart.LoadDataPart(Tenia);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            dataPart.LoadDataPart(Onarys);
        }
    }
}
