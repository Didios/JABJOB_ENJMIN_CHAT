using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollableManager : MonoBehaviour
{
    public List<Transform> toScroll = new List<Transform>();

    [Tooltip("The minimum coordinates")] public Vector3 start;
    [Tooltip("The maximum coordinates")] public Vector3 end;

    public Axis direction;
    public float scrollSpeed;
    /*
    public bool infinite = false;
    */

    void Update()
    {
        var _middle = Input.GetAxis("Mouse ScrollWheel");

        if (_middle != 0)
        {
            Vector3 _first = toScroll[0].position;
            Vector3 _last = toScroll[toScroll.Count -1].position;

            foreach (Transform elmt in toScroll)
            {
                Vector3 _move = new Vector3();

                bool _x = (direction == Axis.X || direction == Axis.XY || direction == Axis.XZ || direction == Axis.XYZ);
                bool _y = (direction == Axis.Y || direction == Axis.XY || direction == Axis.YZ || direction == Axis.XYZ);
                bool _z = (direction == Axis.Z || direction == Axis.XZ || direction == Axis.XZ || direction == Axis.XYZ);

                if (_x &&
                    !(_first.x < transform.position.x + start.x && _middle < 0) &&
                    !(_last.x > transform.position.x + end.x && _middle > 0))
                {
                    _move.x = _middle * scrollSpeed * Time.deltaTime;
                }
                if (_y &&
                    !(_first.y < transform.position.y + start.y && _middle < 0) &&
                    !(_last.y > transform.position.y + end.y && _middle > 0))
                {
                    _move.y = _middle * scrollSpeed * Time.deltaTime;
                }
                if (_z &&
                    !(_first.z < transform.position.z + start.z && _middle < 0) &&
                    !(_last.z > transform.position.z + end.z && _middle > 0))
                {
                    _move.z = _middle * scrollSpeed * Time.deltaTime;
                }

                elmt.Translate(_move);
                /*
                if (infinite)
                {
                    var _newPos = elmt.position;
                    float _dif;

                    if (_x)
                    {
                        if (_newPos.x > transform.position.x + end.x)
                        { _newPos.x = transform.position.x + start.x; }
                        else if (_newPos.x < transform.position.x + start.x)
                        { _newPos.x = transform.position.x + end.x; }
                    }
                    if (_y)
                    {
                        if (_newPos.y > transform.position.y + end.y)
                        {
                            _dif = transform.position.y + end.y - _newPos.y;
                            _newPos.y = transform.position.y + start.y + _dif;
                        }
                        else if (_newPos.y < transform.position.y + start.y)
                        {
                            _dif = transform.position.y + start.y - _newPos.y;
                            _newPos.y = transform.position.y + end.y - _dif;
                        }
                    }
                    if (_z)
                    {
                        if (_newPos.z > transform.position.z + end.z)
                        { _newPos.z = transform.position.z + start.z; }
                        else if (_newPos.z < transform.position.z + start.z)
                        { _newPos.z = transform.position.z + end.z; }
                    }

                    if (_newPos != elmt.position) { elmt.position = _newPos; }
                }
                */
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position + start, 10);
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(transform.position + end, 10);
    }
}
