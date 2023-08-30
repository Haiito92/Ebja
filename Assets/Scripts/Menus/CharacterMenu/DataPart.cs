using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataPart : MonoBehaviour
{

    public GameObject[] tabs;

    [Header("Attributes Tab")]
    [Space]

    public TextMeshProUGUI nameText;

    public TextMeshProUGUI lvlValueText;
    public Image[] elevationMarks;

    public TextMeshProUGUI atkText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI critChanceText;
    public TextMeshProUGUI critDmgText;

    [Space]
    [Header("Lightcone Tab")]
    [Space]

    public TextMeshProUGUI lightconeTab;

    [Space]
    [Header("Traces Tab")]
    [Space]

    public TextMeshProUGUI tracesTab;

    [Space]
    [Header("Relics Tab")]
    [Space]

    public TextMeshProUGUI relicsTab;

    [Space]
    [Header("Eidolons Tab")]
    [Space]

    public TextMeshProUGUI eidolonsTab;

    [Space]
    [Header("Informations Tab")]
    [Space]

    public TextMeshProUGUI informationsTab;

    public void LoadDataPart(CharacterData charData)
    {

        if (charData != null)
        {
            #region LoadAttributes
            nameText.text = charData.name;

            lvlValueText.text = charData.lvl + " / 80";

            for (int i = 0; i < elevationMarks.Length; i++)
            {
                if (i < charData.elevationLvl)
                {
                    elevationMarks[i].color = Color.white;
                }
                else
                {
                    elevationMarks[i].color = Color.red;
                }
            }

            atkText.text = "Atk : " + charData.atk.GetValue();
            defText.text = "Def : " + charData.def.GetValue();
            hpText.text = "Hp : " + charData.hp.GetValue();
            speedText.text = "Speed : " + charData.speed.GetValue();
            critChanceText.text = "CritChance : " + charData.critChance.GetValue();
            critDmgText.text = "CritDmg : " + charData.critDmg.GetValue();
            #endregion

            lightconeTab.text = "LightCone of " + charData.name;
            tracesTab.text = "Traces of " + charData.name;
            relicsTab.text = "Relics of " + charData.name;
            eidolonsTab.text = "Eidolons of " + charData.name;
            informationsTab.text = "Informations of " + charData.name;


        }

    }

    public void CloseAllTabs()
    {
        foreach(GameObject tab in tabs)
        {
            tab.SetActive(false);
        }
    }
}
