using UnityEngine;

namespace ScrollingPlane
{
    public class ScrollingWaterMaterialObject : MonoBehaviour
    {
        private static int textureOffsetPropertyId = Shader.PropertyToID("_WaterTex_ST");
        
        private MeshRenderer meshRenderer;
        private void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();

            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            meshRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetVector(textureOffsetPropertyId, ScrollingWaterMaterialModifier.GetVec4ShaderProperty(textureOffsetPropertyId));
            meshRenderer.SetPropertyBlock(propertyBlock);
            
            ScrollingWaterMaterialModifier.RegisterRenderer(GetComponent<MeshRenderer>());
        }

        private void OnDestroy()
        {
            ScrollingWaterMaterialModifier.DeregisterRenderer(meshRenderer);
        }
    }
}
