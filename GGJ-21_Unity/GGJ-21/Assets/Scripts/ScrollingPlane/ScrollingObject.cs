using UnityEngine;

namespace ScrollingPlane
{
    public class ScrollingObject : MonoBehaviour
    {
        // On Start() because this must always happen later than ScrollingObjectRegistry initialization
        private void Start()
        {
            ScrollingObjectRegistry.Add(this);
        }

        private void OnDestroy()
        {
            ScrollingObjectRegistry.Remove(this);
        }

        public void Move(Vector2 dir, float moveSpeed)
        {
            Vector3 resultMoveVector = -(new Vector3(dir.x, 0f, dir.y));
            transform.position += resultMoveVector * moveSpeed;
        }
    }
}
