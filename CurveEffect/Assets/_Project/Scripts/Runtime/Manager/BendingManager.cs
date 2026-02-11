using EditorAttributes;
using UnityEngine;
using UnityEngine.Rendering;

namespace _Project.Scripts.Runtime.Manager
{
    [ExecuteAlways]
    public class BendingManager : MonoBehaviour
    {
        private const string BendingShaderKeywordName = "_ENABLE_BENDING";
        private const string BendingAmountName = "_BendingAmount";
        [Range(0f,0.1f)]
        public float bendingAmount = 0.015f;

        private GlobalKeyword _bendingShaderKeyword;

        private void Awake()
        {
            _bendingShaderKeyword = GlobalKeyword.Create(BendingShaderKeywordName);
#if UNITY_EDITOR
            if (Application.isPlaying)
                Shader.SetKeyword(_bendingShaderKeyword, true);
            else
                Shader.SetKeyword(_bendingShaderKeyword, false);
#else
                 Shader.SetKeyword(_bendingShaderKeyword, true);
#endif
        }

        private void OnValidate()
        {
            ApplyBendingAmount();
        }


        private void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
            RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
            ApplyBendingAmount();
        }

        private void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
            RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
        }

        private void ApplyBendingAmount()
        {
            Shader.SetGlobalFloat(BendingAmountName, bendingAmount);
        }

        private void OnEndCameraRendering(ScriptableRenderContext ctx, Camera cam)
        {
            //make the custom large orthographic frustum to ensure all bend objects are rendered, even if they are outside the main camera's frustum. The culling matrix is used to determine which objects are rendered
            cam.cullingMatrix = Matrix4x4.Ortho(-99, 99, -99, 99, 0.001f, 99f) * cam.worldToCameraMatrix;
        }

        private void OnBeginCameraRendering(ScriptableRenderContext ctx, Camera cam)
        {
            cam.ResetCullingMatrix();
        }
    }
}