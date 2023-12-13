using UnityEngine;

public class LeaveGame : MonoBehaviour
{
    public void LeaveGameRoom()
    {
        NetworkManager.Instance.LeaveGame();
    }
}
