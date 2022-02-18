using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideableObject : MonoBehaviour
{
    public Transform versionVisible;
    public Transform versionHide;

    public Vector3 posVisible;
    public Vector3 posHide;

    public List<Transform> ObjForHide;
    public float distMaxHide;
    public float distMinHide;
    public float angleMaxHide;

    public bool activate = true;

    // Update is called once per frame
    void Update()
    {
        if (activate)
        {
            bool hide = true;

            foreach(Transform obj in ObjForHide)
            {
                if (Vector3.Distance(posVisible, obj.position) > distMaxHide)
                {
                    hide = false;
                }

                if (Vector3.Angle(obj.forward, posVisible - obj.position) > angleMaxHide)
                {
                    hide = false;
                }

                //Debug.Log(obj.tag);
                //Debug.Log($"D : {Vector3.Distance(posVisible, obj.position)}");
                //Debug.Log($"A : {Vector3.Angle(obj.forward, posVisible - obj.position)}");

                RaycastHit hit;
                if (Physics.Raycast(obj.position, obj.TransformDirection(posVisible - obj.position), out hit, Vector3.Distance(posVisible, obj.position) - distMinHide))
                {
                    hide = false;
                }

                if (!hide) { break; }
            }

            Hide(hide);
        }
    }

    private void Hide(bool hide)
    {
        if (hide)
        {
            versionVisible.position = posHide;
            versionHide.position = posVisible;
        }
        else
        {
            versionHide.position = posHide;
            versionVisible.position = posVisible;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(posVisible, distMaxHide);
        Gizmos.DrawWireSphere(posVisible, distMinHide);

        foreach(Transform t in ObjForHide)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(t.position, posVisible);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(t.position, t.position + t.forward);
        }
    }
}
