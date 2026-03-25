//TEst
using UnityEngine;

public class ExpTest : MonoBehaviour
{
    public ZoneManager zoneManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            zoneManager.AddExp(10);
        }
    }
}
