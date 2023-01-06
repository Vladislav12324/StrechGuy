Shader "Unlit/RopeDual"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Bump ("Normal Map", 2D) = "bump" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _TintColorA("Tint Color A", Color) = (1, 1, 1, 1)
        _TintColorB("Tint Color B", Color) = (1, 1, 1, 1)
        _TintMask("Tint Mask", 2D) = "black"
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv_Bump : TEXCOORD0;
                
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            sampler2D _TintMask;
            float4 _TintMask_ST;
            sampler2D _Bump;
            float4 _Bump_ST;

     
            half _Glossiness;
            half _Metallic;
            float4 _TintColorA;
            float4 _TintColorB;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _TintMask);
                o.uv_Bump = TRANSFORM_TEX(v.uv, _Bump);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 mask = tex2D(_TintMask, i.uv);

                mask = (int)mask;
                half3 d = UnpackNormal(tex2D(_Bump, i.uv_Bump));
                fixed4 col = lerp(_TintColorA, _TintColorB, mask);

                col *= ((half4(d.x, d.y, d.z, 1)));

                return col;
            }
            ENDCG
        }
    }
}
