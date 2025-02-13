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

        if (GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().ArmorAdded > 0)
        {
            gameObject.GetComponent<PlayerController>().dmgReductionProc = armors[armorId].DmgReduction + armors[armorId].DmgReduction *
                (GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().ArmorAdded / 100);
        }
        else
        {
            gameObject.GetComponent<PlayerController>().dmgReductionProc = armors[armorId].DmgReduction;
        }
        

        if(GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().HpAdded > 0)
        {
            if (GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().startHp > 0)
            {
                gameObject.GetComponent<PlayerController>().MaxHp = 200 + (armors[armorId].HpAdd + armors[armorId].HpAdd *
                    (GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().HpAdded / 100));
            }
            else
            {
                gameObject.GetComponent<PlayerController>().MaxHp = 100 + (armors[armorId].HpAdd + armors[armorId].HpAdd *
                    (GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().HpAdded / 100));
            }
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().startHp > 0)
            {
                gameObject.GetComponent<PlayerController>().MaxHp = 200 + armors[armorId].HpAdd;
            }
            else
            {
                gameObject.GetComponent<PlayerController>().MaxHp = 100 + armors[armorId].HpAdd;
            }
        }

        if (GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().DodgeAdded > 0)
        {
            gameObject.GetComponent<PlayerController>().dodgeChance = armors[armorId].DodgeChance + armors[armorId].DodgeChance *
                (GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().DodgeAdded / 100);
        }
        else
        {
            gameObject.GetComponent<PlayerController>().dodgeChance = armors[armorId].DodgeChance;
        }


        gameObject.GetComponent<PlayerController>().speed = 10 + armors[armorId].SpeedAdded;
        gameObject.GetComponent<PlayerController>().UpdateArmorInfo(currentArmor);
    }
}
