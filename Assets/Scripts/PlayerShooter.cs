using UnityEngine;
using System.Collections.Generic; // Để dùng List

public class PlayerShooter : MonoBehaviour
{
    [Header("Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint; // Tạo 1 empty object con của Player, đặt ở đầu súng
    public float bulletSpeed = 20f;
    public int maxBounces = 3; // Số lần nảy tối đa dự đoán
    public float maxStepDistance = 30f; // Độ dài tối đa của đường vẽ
    public LayerMask obstacleLayer; // Layer của Tường và Enemy

    private LineRenderer _lineRenderer;
    private bool _isAiming = false;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
    }

    void Update()
    {
        // 1. Nhấn chuột: Bắt đầu ngắm
        if (Input.GetMouseButtonDown(0))
        {
            _isAiming = true;
            _lineRenderer.enabled = true;
        }

        // 2. Giữ chuột: Vẽ đường đạn
        if (_isAiming && Input.GetMouseButton(0))
        {
            DrawTrajectory();
        }

        // 3. Thả chuột: Bắn
        if (Input.GetMouseButtonUp(0) && _isAiming)
        {
            _isAiming = false;
            _lineRenderer.enabled = false;
            Shoot();
        }
    }

    void DrawTrajectory()
    {
        // Tính hướng bắn dựa trên vị trí chuột so với Player
        Vector3 mousePos = GetMouseWorldPosition();
        Vector3 direction = (mousePos - transform.position).normalized;
        direction.y = 0; // Giữ phẳng trên mặt đất

        // Xoay Player theo hướng bắn
        if (direction != Vector3.zero)
            transform.forward = direction;

        // BẮT ĐẦU TÍNH TOÁN RAYCAST
        List<Vector3> points = new List<Vector3>();
        points.Add(firePoint.position); // Điểm đầu tiên là nòng súng

        Vector3 currentPosition = firePoint.position;
        Vector3 currentDirection = direction;

        for (int i = 0; i < maxBounces; i++)
        {
            Ray ray = new Ray(currentPosition, currentDirection);
            RaycastHit hit;

            // Bắn tia ray đi xem trúng cái gì
            if (Physics.Raycast(ray, out hit, maxStepDistance, obstacleLayer))
            {
                points.Add(hit.point); // Thêm điểm va chạm vào list vẽ
                
                // Toán học: Tính góc phản xạ (Reflect)
                // hit.normal là vector pháp tuyến của bề mặt va chạm
                currentDirection = Vector3.Reflect(currentDirection, hit.normal);
                currentPosition = hit.point;

                // Nếu trúng Enemy thì dừng vẽ luôn, không nảy nữa
                if (hit.collider.CompareTag("Enemy"))
                {
                    break; 
                }
            }
            else
            {
                // Nếu không trúng gì thì vẽ thẳng tắp ra xa rồi dừng
                points.Add(currentPosition + currentDirection * maxStepDistance);
                break;
            }
        }

        // Cập nhật LineRenderer
        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(points.ToArray());
    }

    void Shoot()
    {
        // Tạo đạn
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        
        // Bắn theo hướng Player đang nhìn
        rb.linearVelocity = transform.forward * bulletSpeed;
        
        // Hủy đạn sau 5s nếu không trúng gì
        Destroy(bullet, 5f);
    }

    // Hàm phụ trợ để lấy vị trí chuột trong thế giới 3D
    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // Mặt phẳng đất ảo y=0
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            return ray.GetPoint(rayDistance);
        }
        return transform.position + transform.forward;
    }
}