using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// task bar behavior
/// </summary>
public class taskBar : MonoBehaviour
{
    public GameObject green;
    // Update is called once per frame
    void FixedUpdate()
    {
        float pos = Interact.taskProgress * 6 -600;
        Vector2 gpos = green.transform.localPosition;
        if (gpos.x < pos)
        {
            green.transform.localPosition = new Vector2(gpos.x + 5, gpos.y);
            if (green.transform.localPosition.x >= pos)
            {
                green.transform.localPosition = new Vector2(pos, gpos.y);
            }
        }
        else if (gpos.x > pos)
        {
            green.transform.localPosition = new Vector2(gpos.x - 5, gpos.y);
            if (green.transform.localPosition.x <= pos)
            {
                green.transform.localPosition = new Vector2(pos, gpos.y);
            }
        }
    }
}
