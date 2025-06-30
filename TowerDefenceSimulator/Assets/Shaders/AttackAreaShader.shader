Shader "Unlit/AttackAreaShader"
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
        _FillAmount ("FillAmount", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _AlphaTex;
            sampler2D _EmissionTex;
            float4 _BaseColor;
            float4 _EmissionColor;
            float _EmissionPower;
            float _AphaPower;
            float _FillAmount;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
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
                float2 uv = i.uv - float2(0.5, 0.5); //���S��ɏC��
                float angle = atan2(uv.y, uv.x); // [-PI, PI]
                float normalizedAngle = (angle + UNITY_PI) / (2.0 * UNITY_PI); // [0, 1]

                float fill = step(normalizedAngle, _FillAmount);

                //�����F�̔䗦���Z�o
                float emissionScale = tex2D(_EmissionTex, i.uv).r;
                
                float3 emission = _EmissionColor.rgb * emissionScale * _EmissionPower;

                //�����̔䗦���Z�o
                float alphaScale = tex2D(_AlphaTex, i.uv).r;

                float alpha = alphaScale * _AphaPower;

                //�`�掞�̐F�ɔ����E��K��
                float3 finalColor = _BaseColor.rgb + emission;

                return float4(finalColor, alpha) *= fill;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
