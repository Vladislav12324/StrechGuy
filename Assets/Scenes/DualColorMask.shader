Shader "Custom/DualColorMask" {
    Properties {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Bump ("Normal Map", 2D) = "bump" {}
        _TintColorA("Tint Color A", Color) = (1, 1, 1, 1)
        _TintColorB("Tint Color B", Color) = (1, 1, 1, 1)
        _TintMask("Tint Mask", 2D) = "black"
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
     
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
 
        #pragma target 3.0
 
        sampler2D _TintMask;
        sampler2D _MainTex;
        sampler2D _Bump;
 
        struct Input {
            float2 uv_TintMask;
            float2 uv_Bump;
        };
 
        float4 _TintColorA;
        float4 _TintColorB;
 
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
 
            float4 mask = tex2D(_TintMask, IN.uv_TintMask);

            mask = (int)mask;

            o.Albedo = lerp(_TintColorA, _TintColorB, mask);
            
            o.Alpha = mask.a;
            o.Normal = UnpackNormal(tex2D(_Bump, IN.uv_Bump) * 1.15);
        }
        ENDCG
    }
    FallBack "Diffuse"
}