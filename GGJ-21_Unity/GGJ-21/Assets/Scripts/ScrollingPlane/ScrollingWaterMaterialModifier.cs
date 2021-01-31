using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ScrollingPlane
{
    public class ScrollingWaterMaterialModifier : MonoBehaviour
    {
        private static ScrollingWaterMaterialModifier instance;
        
        private List<MeshRenderer> waterMaterialRenderers = new List<MeshRenderer>();

        private void Awake()
        {
            Assert.IsNull(instance, "There can be only one ScrollingWaterMaterialModifier in the scene!");
            instance = this;
        }

        public static void RegisterRenderer(MeshRenderer meshRenderer)
        {
            Assert.IsFalse(instance.waterMaterialRenderers.Contains(meshRenderer), "Already registered!");
            instance.waterMaterialRenderers.Add(meshRenderer);
        }

        public static void DeregisterRenderer(MeshRenderer meshRenderer)
        {
            Assert.IsTrue(instance.waterMaterialRenderers.Contains(meshRenderer), "Trying to deregister non-existent object!");
            instance.waterMaterialRenderers.Remove(meshRenderer);
        }

        public static void ModifyVec4ShaderProperty(int vectorProperty, Vector4 value)
        {
            foreach (MeshRenderer meshRenderer in instance.waterMaterialRenderers)
            {
                MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
                meshRenderer.GetPropertyBlock(propertyBlock);
                propertyBlock.SetVector(vectorProperty, value);
                meshRenderer.SetPropertyBlock(propertyBlock);
            }
        }

        public static Vector4 GetVec4ShaderProperty(int vectorProperty)
        {
            MeshRenderer prevMeshRenderer = null;
            Vector4 prevVecValue = Vector4.zero;
            foreach (MeshRenderer meshRenderer in instance.waterMaterialRenderers)
            {
                MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
                meshRenderer.GetPropertyBlock(propertyBlock);
                Vector4 result = propertyBlock.GetVector(vectorProperty);
                if (prevMeshRenderer == null)
                {
                    prevMeshRenderer = meshRenderer;
                    prevVecValue = result;
                    continue;
                }
                
                Assert.AreEqual(prevVecValue, result, "Some values are across renderers not equal!");
                prevMeshRenderer = meshRenderer;
                prevVecValue = result;
            }

            return prevVecValue;
        }
    }
}
