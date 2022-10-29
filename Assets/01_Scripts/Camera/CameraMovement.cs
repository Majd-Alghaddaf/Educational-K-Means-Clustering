using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] float orbitSpeed = 5f;
    [SerializeField] float zoomSpeed = 5f;

    private Camera _camera;
    private Vector3 _orbitCenter = Vector3.zero;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(2))
        {
            Orbit();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            Zoom(Input.GetAxis("Mouse ScrollWheel") * zoomSpeed);
        }
    }

    private void Orbit()
    {
        Vector3 cross = Vector3.Cross(Vector3.Normalize(_camera.transform.position - _orbitCenter), Vector3.up);
        Vector3 crossOnPlane = new Vector3(cross.x, 0, cross.z);

        _camera.transform.RotateAround(_orbitCenter, Vector3.up, Input.GetAxis("Mouse X") * orbitSpeed);

        if ((cross.magnitude > 0.1f) ||
            (cross.magnitude <= 0.1f && Vector3.Dot(_camera.transform.position - _orbitCenter, Vector3.up) >= 0 && Input.GetAxis("Mouse Y") > 0) ||
            (cross.magnitude <= 0.1f && Vector3.Dot(_camera.transform.position - _orbitCenter, Vector3.up) < 0 && Input.GetAxis("Mouse Y") < 0))
        {

            if (crossOnPlane.magnitude > 0.1f)
            {
                _camera.transform.RotateAround(_orbitCenter, crossOnPlane, -Input.GetAxis("Mouse Y") * orbitSpeed);
            }
            else
            {
                if (crossOnPlane.magnitude == 0)
                {
                    if (Mathf.Abs(_camera.transform.rotation.eulerAngles.y) < 90.0f)
                    {
                        _camera.transform.RotateAround(_orbitCenter, Vector3.right, -Input.GetAxis("Mouse Y") * orbitSpeed);

                    }
                    else
                    {
                        _camera.transform.RotateAround(_orbitCenter, Vector3.left, -Input.GetAxis("Mouse Y") * orbitSpeed);

                    }
                }
                else
                {
                    _camera.transform.RotateAround(_orbitCenter, Vector3.Normalize(crossOnPlane), -Input.GetAxis("Mouse Y") * orbitSpeed);

                }
            }

            _camera.transform.LookAt(_orbitCenter);
        }
    }

    private void Zoom(float input)
    {
        if (Vector3.Distance(_orbitCenter, transform.position) < 1f)
        {
            _camera.transform.Translate(Vector3.back * 2f);
            return;
        }

        _camera.transform.LookAt(_orbitCenter);
        _camera.transform.Translate(Vector3.forward * input);
    }
}
