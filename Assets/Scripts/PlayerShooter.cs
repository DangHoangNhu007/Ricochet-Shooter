using UnityEngine;
using System.Collections.Generic; 

public class PlayerShooter : MonoBehaviour
{
    [Header("Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint; 
    public float bulletSpeed = 20f;
    public int maxBounces = 3;
    public float maxStepDistance = 30f; 
    public LayerMask obstacleLayer; 

    private LineRenderer _lineRenderer;
    private bool _isAiming = false;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isAiming = true;
            _lineRenderer.enabled = true;
        }

        if (_isAiming && Input.GetMouseButton(0))
        {
            DrawTrajectory();
        }

        if (Input.GetMouseButtonUp(0) && _isAiming)
        {
            _isAiming = false;
            _lineRenderer.enabled = false;
            Shoot();
        }
    }

    void DrawTrajectory()
    {
        Vector3 mousePos = GetMouseWorldPosition();
        Vector3 direction = (mousePos - transform.position).normalized;
        direction.y = 0; 

        if (direction != Vector3.zero)
            transform.forward = direction;

        List<Vector3> points = new List<Vector3>();
        points.Add(firePoint.position); 
        Vector3 currentPosition = firePoint.position;
        Vector3 currentDirection = direction;

        for (int i = 0; i < maxBounces; i++)
        {
            Ray ray = new Ray(currentPosition, currentDirection);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxStepDistance, obstacleLayer))
            {
                points.Add(hit.point); 
                
                currentDirection = Vector3.Reflect(currentDirection, hit.normal);
                currentPosition = hit.point;

                if (hit.collider.CompareTag("Enemy"))
                {
                    break; 
                }
            }
            else
            {
                points.Add(currentPosition + currentDirection * maxStepDistance);
                break;
            }
        }

        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(points.ToArray());
    }

    void Shoot()
    {
        AudioManagerShooter.Instance.PlayShoot();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        
        rb.linearVelocity = transform.forward * bulletSpeed;
        
        Destroy(bullet, 5f);
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); 
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            return ray.GetPoint(rayDistance);
        }
        return transform.position + transform.forward;
    }
}