using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataPart : MonoBehaviour
{

    public GameObject[] tabs;

    #region AttributesTabVariables
    [Header("Attributes Tab")]
    [Space]

    public TextMeshProUGUI nameText;

    public TextMeshProUGUI lvlValueText;
    public ElevationMark[] elevationMarks;

    public TextMeshProUGUI atkText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI critChanceText;
    public TextMeshProUGUI critDmgText;
    #endregion

    #region EnergySourceTabVariables
    [Space]
    [Header("EnergySource Tab")]
    [Space]

    public Image esIcon;
    public TextMeshProUGUI esNameText;
    public TextMeshProUGUI esTypeText;

    public TextMeshProUGUI esEffectText;

    public TextMeshProUGUI esLevelValueText;
    public ElevationMark[] superpositionMarks;

    public TextMeshProUGUI esAtktext;
    public TextMeshProUGUI esDeftext;
    public TextMeshProUGUI esHptext;

    #endregion

    #region TracesTabVariables
    [Space]
    [Header("Traces Tab")]
    [Space]

    public TextMeshProUGUI tracesTab;
    #endregion

    #region RelicsTabVariables
    [Space]
    [Header("Relics Tab")]
    [Space]

    public TextMeshProUGUI relicTabAtkText;
    public TextMeshProUGUI relicTabDefText;
    public TextMeshProUGUI relicTabHpText;
    public TextMeshProUGUI relicTabSpeedText;
    public TextMeshProUGUI relicTabCritChanceText;
    public TextMeshProUGUI relicTabCritDmgText;

    #endregion

    #region EidolonsTabVariables
    [Space]
    [Header("Eidolons Tab")]
    [Space]

    public TextMeshProUGUI eidolonsTab;
    #endregion

    #region InformationsTabVariables
    [Space]
    [Header("Informations Tab")]
    [Space]

    public TextMeshProUGUI informationsTab;
    #endregion

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
                    elevationMarks[i].elevationMarkIcon.sprite = elevationMarks[i].fullMark;
                }
                else
                {
                    elevationMarks[i].elevationMarkIcon.sprite = elevationMarks[i].emptyMark;
                }
            }

            atkText.text = "Atk : " + charData.atk.GetValue();
            defText.text = "Def : " + charData.def.GetValue();
            hpText.text = "Hp : " + charData.hp.GetValue();
            speedText.text = "Speed : " + charData.speed.GetValue();
            critChanceText.text = "CritChance : " + charData.critChance.GetValue();
            critDmgText.text = "CritDmg : " + charData.critDmg.GetValue();
            #endregion

            #region LoadEnergySource

            esNameText.text = charData.energySource.name;
            esTypeText.text = charData.energySource.typeName;
            esTypeText.color = charData.energySource.typeColor;
            esIcon.sprite = charData.energySource.icon;

            esEffectText.text = charData.energySource.esEffect;

            esLevelValueText.text = charData.energySource.lvl.ToString() + " / 80";
            for (int i = 0; i < superpositionMarks.Length; i++)
            {
                if (i < charData.energySource.superpositionLvl)
                {
                    superpositionMarks[i].elevationMarkIcon.sprite = superpositionMarks[i].fullMark;
                }
                else
                {
                    superpositionMarks[i].elevationMarkIcon.sprite = superpositionMarks[i].emptyMark;
                }


            }


            esAtktext.text = "Atk : " + charData.energySource.atk.GetValue();
            esDeftext.text = "Def : " + charData.energySource.def.GetValue();
            esHptext.text = "Hp : " + charData.energySource.hp.GetValue();

            #endregion


            #region LoeadRelicsTab

            relicTabAtkText.text = "Atk : " + charData.atk.GetValue();
            relicTabDefText.text = "Def : " + charData.def.GetValue();
            relicTabHpText.text = "Hp : " + charData.hp.GetValue();
            relicTabSpeedText.text = "Speed : " + charData.speed.GetValue();
            relicTabCritChanceText.text = "CritChance : " + charData.critChance.GetValue();
            relicTabCritDmgText.text = "CritDmg : " + charData.critDmg.GetValue();

        #endregion

        #region Undone
        tracesTab.text = "Traces of " + charData.name;
            
            eidolonsTab.text = "Eidolons of " + charData.name;
            informationsTab.text = "Informations of " + charData.name;
        #endregion

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
