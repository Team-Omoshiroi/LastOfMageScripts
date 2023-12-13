using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/AddMoveSpeed")]
public class MoveSpeedSO : CharacterStatModifierSO
{
    bool isBufe;
    public override void AffectCharacter(GameObject character, float val)
    {
        DataContainer data = character.GetComponent<DataContainer>();
        if (data != null)
        {
            MonoBehaviour monoBehaviour = character.GetComponent<MonoBehaviour>();
            if (monoBehaviour != null)
            {
                if(isBufe == false)
                {
                 
                    monoBehaviour.StartCoroutine(AddMoveSpeed(data, val));
                }
                else
                {
                    monoBehaviour.StopCoroutine(AddMoveSpeed(data, val));
                    monoBehaviour.StartCoroutine(AddMoveSpeed(data, val));
                }
               
            }
            else
            {
                Debug.LogWarning("NullMonoBehaviour ");
            }
        }
        else
        {
            Debug.LogWarning("NullDataContainer");
        }
    }

  

    private IEnumerator AddMoveSpeed(DataContainer data, float addspeed)
    {  
        data.Stats.MoveSpeed += addspeed;
        yield return new WaitForSeconds(10f);
        data.Stats.MoveSpeed -= addspeed;
        isBufe = true;


    }
}