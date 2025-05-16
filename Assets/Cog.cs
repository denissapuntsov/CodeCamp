using UnityEngine;

public class Cog : MonoBehaviour
{
    public void Connect()
    {
        GetComponent<Animator>().Play("CogConnected");
    }
}
