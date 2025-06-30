Shader "Custom/TextureShader"
{
//パラメータ--------------------------------------------------------------------------------
    //インスペクターで表示するプロパティを定義
    Properties{
        //参照するスプラットマップ
        [NoScaleOffset] _SelectionMap("Selection Map", 2D) = "red" {}
        //スプラットマップ一枚に対する適応範囲
        _SelectionArea("Selection Area (MinX, MinZ, MaxX, MaxZ)", Vector) = (0, 0, 100, 100)

        _Cutoff("Alpha Cutoff (Valid In Cutout Mode)", Range(0,1)) = 0.5

            /*- 適応するテクスチャ情報① -*/
        _Color("[1] Color", Color) = (1,1,1,1)
        _MainTex("[1] Albedo (RGB)", 2D) = "white" {}
        _Glossiness("[1] Smoothness", Range(0,1)) = 0.5
        [Gamma] _Metallic("[1] Metallic Scale", Range(0,1)) = 0.0
        [NoScaleOffset]_MetallicGlossMap("[1] Metallic", 2D) = "white" {}
        _BumpScale("[1] Normal Scale", Float) = 1.0
        [NoScaleOffset][Normal] _BumpMap("[1] Normal Map", 2D) = "bump" {}
        [HDR] _EmissionColor("[1] Emittion Color", Color) = (0, 0, 0, 0)
        [NoScaleOffset] _EmissionTex("[1] Emission", 2D) = "white" {}
        _OcclusionStrength("[1] Occlusion Strength", Range(0.0, 1.0)) = 1.0
        [NoScaleOffset] _OcclusionMap("[1] Occlusion", 2D) = "white" {}

           /*- 適応するテクスチャ情報② -*/
        _Color2("[2] Color", Color) = (1,1,1,1)
        [NoScaleOffset] _MainTex2("[2] Albedo (RGB)", 2D) = "white" {}
        _Glossiness2("[2] Smoothness", Range(0,1)) = 0.5
        [Gamma] _Metallic2("[2] Metallic Scale", Range(0,1)) = 0.0
        [HDR] _EmissionColor2("[2] Emittion Color", Color) = (0, 0, 0, 0)
        [NoScaleOffset] _EmissionTex2("[2] Emission", 2D) = "white" {}
        _OcclusionStrength2("[2] Occlusion Strength", Range(0.0, 1.0)) = 1.0

           /*- 適応するテクスチャ情報③ -*/
        _Color3("[3] Color", Color) = (1,1,1,1)
        [NoScaleOffset] _MainTex3("[3] Albedo (RGB)", 2D) = "white" {}
        _Glossiness3("[3] Smoothness", Range(0,1)) = 0.5
        [Gamma] _Metallic3("[3] Metallic Scale", Range(0,1)) = 0.0
        [HDR] _EmissionColor3("[3] Emittion Color", Color) = (0, 0, 0, 0)
        [NoScaleOffset] _EmissionTex3("[3] Emission", 2D) = "white" {}
        _OcclusionStrength3("[3] Occlusion Strength", Range(0.0, 1.0)) = 1.0
    }
//----------------------------------------------------------------------------------------

    CGINCLUDE
    
    ENDCG

//シェーダ設定----------------------------------------------------------------------------
    SubShader{
        Tags { "Queue" = "Geometry" "RenderType" = "Opaque" }
        LOD 200
        CGPROGRAM

        //surf関数を実際の処理時に呼び出すシェーダ関数として設定
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

            /*- 変数定義 -*/

        sampler2D _SelectionMap;
        float4 _SelectionArea;

        fixed4 _Color;
        sampler2D _MainTex;
        half _BumpScale;
        sampler2D _BumpMap;
        fixed4 _EmissionColor;
        sampler2D _EmissionTex;
        half _OcclusionStrength;
        sampler2D _OcclusionMap;
        half _Glossiness;
        half _Metallic;
        sampler2D _MetallicGlossMap;

        fixed4 _Color2;
        sampler2D _MainTex2;
        fixed4 _EmissionColor2;
        sampler2D _EmissionTex2;
        half _OcclusionStrength2;
        half _Glossiness2;
        half _Metallic2;

        fixed4 _Color3;
        sampler2D _MainTex3;
        fixed4 _EmissionColor3;
        sampler2D _EmissionTex3;
        half _OcclusionStrength3;
        half _Glossiness3;
        half _Metallic3;

            /*- 構造体定義 -*/

        struct Input {
            float2 uv_MainTex;
            float3 worldPos;    //ワールド座標
        };

        //入力変数をまとめた構造体
        struct MaterialSetting {
            fixed4 Color;
            sampler2D MainTex;
            half BumpScale;
            sampler2D BumpMap;
            fixed4 EmissionColor;
            sampler2D EmissionTex;
            half OcclusionStrength;
            sampler2D OcclusionMap;
            half Glossiness;
            half Metallic;
            sampler2D MetallicGlossMap;
        };


        //cgincファイルをインクルード
         #include "cginc/DemoShader.cginc"
        /*
            cgincファイル
                シェーダファイルにおけるヘッダーファイルのようなもの
                ヘッダーファイルとは違い、宣言だけでなく定義まで行う
                includeを記述した箇所に展開される
        */
            /*
                Input構造体 (Vertexシェーダから出力された値)
                    ・uv_MainTex //テクスチャのUV座標
                    ・viewDir    //視線方向
                    ・worldPos   //ワールド座標
                    ・screenPos  //スクリーン座標

                input ScurfaceOutputStanderd構造体 (オブジェクトの表面色)
                    ・fixed3 Albedo      //基本色
                    ・fixed3 Normal      //法線情報
                    ・half3  Emission
                    ・half   Metallic    //0 = 非メタル / 1 = メタル
                    ・half   Smoothness  //0 = 粗い / 1 = 滑らか
                    ・half   Occlusion   //オクルージョン(デフォルト 1)
                    ・fixed  Alpha       //透明度のアルファ
        */
        ENDCG
    }

    Fallback "Standard"
}
