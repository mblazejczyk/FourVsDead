using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorController : MonoBehaviour
{
    public ArmorSO[] armors;
    public int currentArmor = 0;

    public void SetNewArmor(int armorId)
    {
        currentArmor = armorId;
        gameObject.GetComponent<PlayerController>().dmgReductionProc = armors[armorId].DmgReduction;
        gameObject.GetComponent<PlayerController>().MaxHp = 100 + armors[armorId].HpAdd;
        gameObject.GetComponent<PlayerController>().dodgeChance = armors[armorId].DodgeChance;
        gameObject.GetComponent<PlayerController>().speed = 10 + armors[armorId].SpeedAdded;
        gameObject.GetComponent<PlayerController>().UpdateArmorInfo(currentArmor);
    }
}
