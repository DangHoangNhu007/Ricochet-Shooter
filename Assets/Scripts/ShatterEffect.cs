using UnityEngine;

public class ShatterEffect : MonoBehaviour
{
    [Header("Settings")]
    public int pieceCount = 10; 
    public float explosionForce = 15f; 
    public float scaleSize = 0.4f; 

    public void Explode()
    {
        // 1. Lấy màu của đối tượng hiện tại
        Color objectColor = Color.white; 
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            objectColor = rend.material.color;
            rend.enabled = false; // Ẩn object chính
        }
        
        // 2. Tắt Collider
        Collider col = GetComponent<Collider>();
        if(col != null) col.enabled = false;

        // 3. Sinh mảnh vỡ
        for (int i = 0; i < pieceCount; i++)
        {
            SpawnPiece(objectColor);
        }

        // 4. Hủy object gốc sau 2s
        Destroy(gameObject, 2f);
    }

    void SpawnPiece(Color color)
    {
        GameObject piece = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        // Vị trí ngẫu nhiên quanh tâm
        Vector3 randomPos = transform.position + Random.insideUnitSphere * 0.5f;
        piece.transform.position = randomPos;
        piece.transform.localScale = Vector3.one * scaleSize;

        // Gán màu
        piece.GetComponent<Renderer>().material.color = color;

        // Vật lý
        Rigidbody rb = piece.AddComponent<Rigidbody>();
        rb.mass = 0.2f;
        
        // Lực nổ hướng ra ngoài từ tâm
        Vector3 forceDir = (piece.transform.position - transform.position).normalized;
        rb.AddForce(forceDir * explosionForce, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);

        // Tự hủy mảnh
        Destroy(piece, 2.5f);
    }
}