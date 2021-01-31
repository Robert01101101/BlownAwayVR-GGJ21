using UnityEngine;

namespace ScrollingPlane
{
    public class ScrollingPlaneWater : MonoBehaviour
    {
        private static int tilingPropertyId = Shader.PropertyToID("_Tiling");
        private static int textureOffsetPropertyId = Shader.PropertyToID("_WaterTex_ST");
        
        [SerializeField]
        private MeshRenderer plane = default;

        public float WaterTile => plane.sharedMaterial.GetVector(tilingPropertyId).x;

        public void Move(Vector2 dir, float moveSpeed)
        {
            Vector2 deltaVec2Offset = -dir.normalized * moveSpeed;
            Vector4 currentVec4Offset = ScrollingWaterMaterialModifier.GetVec4ShaderProperty(textureOffsetPropertyId);
            Vector4 newVec4Offset = new Vector4(0f, 0f, deltaVec2Offset.x, deltaVec2Offset.y) + currentVec4Offset;
            newVec4Offset.x = 1f;
            newVec4Offset.y = 1f;

            ScrollingWaterMaterialModifier.ModifyVec4ShaderProperty(textureOffsetPropertyId, newVec4Offset);
        }
    }
}
