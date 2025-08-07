using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class MeshColliderDrawer : MonoBehaviour
{
    public Color gizmoColor = Color.green;

    void OnDrawGizmos()
    {
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider == null || meshCollider.sharedMesh == null)
            return;

        Gizmos.color = gizmoColor;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireMesh(meshCollider.sharedMesh);
    }
}