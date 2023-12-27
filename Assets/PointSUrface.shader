Shader "Graph/PointSUrface"
{
    Properties
    {
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface ConfigureSurface Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
            float3 worldPos;
        };
        
        float _Smoothness;
        //inout indicates that it's both passed to the function and used for the result of the function.
        void ConfigureSurface(Input input, inout SurfaceOutputStandard surface) 
        {
            surface.Albedo.rg = (input.worldPos.xy *0.5) +0.5;
            surface.Smoothness = _Smoothness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
