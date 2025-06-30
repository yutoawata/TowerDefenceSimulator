Shader "Unlit/HologramShader"
{
    Properties
    {
        _AlphaTex ("AlphaTexture", 2D) = "white" {}
        _EmissionTex ("EmissionTexture", 2D) = "white" {}
        [HDR]
        _BaseColor ("BaseColor", Color) = (1, 1, 1, 1)
        [HDR]
        _EmissionColor ("EmissionColor", Color) = (1, 1, 1, 1)
        _EmissionPower ("EmissionPower", Range(0, 2)) = 1
        _AphaPower ("AlhaPower", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue" ="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        LOD 100
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            sampler2D _AlphaTex;
            sampler2D _EmissionTex;
            float4 _BaseColor;
            float4 _EmissionColor;
            float _EmissionPower;
            float _AphaPower;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //î≠åıêFÇÃî‰ó¶ÇéZèo
                float emissionScale = tex2D(_EmissionTex, i.uv).r;
                
                float3 emission = _EmissionColor.rgb * emissionScale * _EmissionPower;

                //ìôâøÇÃî‰ó¶ÇéZèo
                float alphaScale = tex2D(_AlphaTex, i.uv).r;

                float alpha = alphaScale * _AphaPower;

                //ï`âÊéûÇÃêFÇ…î≠åıêEÇìKâû
                float3 finalColor = _BaseColor.rgb + emission;

                return float4(finalColor, alpha);
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
