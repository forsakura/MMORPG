using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameObject
{
    public class SpawnPoint : MonoBehaviour
    {
        public int ID;
        public Mesh mesh;
        // Use this for initialization
        void Start()
        {
            mesh = GetComponent<MeshFilter>().sharedMesh;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Color color = Color.green;
            if (mesh != null)
            {
                Gizmos.DrawWireMesh(mesh, transform.position, Quaternion.identity, Vector3.one);
            }
            UnityEditor.Handles.color = color;
            UnityEditor.Handles.ArrowHandleCap(0, transform.position, transform.rotation, 2f, EventType.Repaint);
            UnityEditor.Handles.Label(transform.position + Vector3.up * 1f, "SpawnPoint:" + this.ID);
        }
#endif
    }
}