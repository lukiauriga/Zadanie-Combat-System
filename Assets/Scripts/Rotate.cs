using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public void RotateSprite()
    {
        transform.Rotate(0, 8.2f, 0);
        // Mniej wiêcej pe³na rotacja, offset naprawiony jest przez FixAngle
    }

    public void FixAngle()
    {
        transform.eulerAngles = new Vector3(90, 0, 0);
    }
}
