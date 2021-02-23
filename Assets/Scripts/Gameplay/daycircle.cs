using UnityEngine;
using System.Collections;

public class daycircle : MonoBehaviour {
    public bool Day = true;
    float _t;
    public Vector3 Axis = Vector3.right;
    void LateUpdate()
    {
        _t += Time.deltaTime;
        this.transform.Rotate(Axis * Time.deltaTime);
        if (_t >= 180 / Axis.x)
        {
            Day = !Day;
            _t = 0;
            return;
        }
    }
}