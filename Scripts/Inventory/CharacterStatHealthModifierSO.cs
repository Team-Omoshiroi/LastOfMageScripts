using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/AddHP")]
public class CharacterStatHealthModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        HealthSystem health = character.GetComponent<HealthSystem>();
        if (health != null)
        {
            Debug.Log("회복!");
            health.TakeRecovery((int)val);
        }
        else
        {
            Debug.LogError("NUllHealthSystem");
        }
       
    }
}