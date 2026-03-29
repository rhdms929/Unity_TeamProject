using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ZoneButtonUI : MonoBehaviour
{
    public ZoneManager zoneManager;
    public int zoneIndex;

    public void OnClickZoneButton()
    {
        zoneManager.MoveToZone(zoneIndex);
    }
}