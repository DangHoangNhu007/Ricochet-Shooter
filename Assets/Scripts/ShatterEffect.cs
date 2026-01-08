using UnityEngine;

public class ShatterEffect : MonoBehaviour
{
    [Header("Settings")]
    public int pieceCount = 10; 
    public float explosionForce = 15f; 
    public float scaleSize = 0.4f; 

    public void Explode()
    {
        Color objectColor = Color.white; 
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            objectColor = rend.material.color;
            rend.enabled = false; 
        }
        
        Collider col = GetComponent<Collider>();
        if(col != null) col.enabled = false;

        for (int i = 0; i < pieceCount; i++)
        {
            SpawnPiece(objectColor);
        }

        Destroy(gameObject, 2f);
    }

    void SpawnPiece(Color color)
    {
        GameObject piece = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        Vector3 randomPos = transform.position + Random.insideUnitSphere * 0.5f;
        piece.transform.position = randomPos;
        piece.transform.localScale = Vector3.one * scaleSize;

        piece.GetComponent<Renderer>().material.color = color;

        Rigidbody rb = piece.AddComponent<Rigidbody>();
        rb.mass = 0.2f;
        
        Vector3 forceDir = (piece.transform.position - transform.position).normalized;
        rb.AddForce(forceDir * explosionForce, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);

        Destroy(piece, 2.5f);
    }
}