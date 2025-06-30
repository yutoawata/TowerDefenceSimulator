Shader "Custom/TextureShader"
{
//�p�����[�^--------------------------------------------------------------------------------
    //�C���X�y�N�^�[�ŕ\������v���p�e�B���`
    Properties{
        //�Q�Ƃ���X�v���b�g�}�b�v
        [NoScaleOffset] _SelectionMap("Selection Map", 2D) = "red" {}
        //�X�v���b�g�}�b�v�ꖇ�ɑ΂���K���͈�
        _SelectionArea("Selection Area (MinX, MinZ, MaxX, MaxZ)", Vector) = (0, 0, 100, 100)

        _Cutoff("Alpha Cutoff (Valid In Cutout Mode)", Range(0,1)) = 0.5

            /*- �K������e�N�X�`�����@ -*/
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

           /*- �K������e�N�X�`�����A -*/
        _Color2("[2] Color", Color) = (1,1,1,1)
        [NoScaleOffset] _MainTex2("[2] Albedo (RGB)", 2D) = "white" {}
        _Glossiness2("[2] Smoothness", Range(0,1)) = 0.5
        [Gamma] _Metallic2("[2] Metallic Scale", Range(0,1)) = 0.0
        [HDR] _EmissionColor2("[2] Emittion Color", Color) = (0, 0, 0, 0)
        [NoScaleOffset] _EmissionTex2("[2] Emission", 2D) = "white" {}
        _OcclusionStrength2("[2] Occlusion Strength", Range(0.0, 1.0)) = 1.0

           /*- �K������e�N�X�`�����B -*/
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

//�V�F�[�_�ݒ�----------------------------------------------------------------------------
    SubShader{
        Tags { "Queue" = "Geometry" "RenderType" = "Opaque" }
        LOD 200
        CGPROGRAM

        //surf�֐������ۂ̏������ɌĂяo���V�F�[�_�֐��Ƃ��Đݒ�
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

            /*- �ϐ���` -*/

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

            /*- �\���̒�` -*/

        struct Input {
            float2 uv_MainTex;
            float3 worldPos;    //���[���h���W
        };

        //���͕ϐ����܂Ƃ߂��\����
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


        //cginc�t�@�C�����C���N���[�h
         #include "cginc/DemoShader.cginc"
        /*
            cginc�t�@�C��
                �V�F�[�_�t�@�C���ɂ�����w�b�_�[�t�@�C���̂悤�Ȃ���
                �w�b�_�[�t�@�C���Ƃ͈Ⴂ�A�錾�����łȂ���`�܂ōs��
                include���L�q�����ӏ��ɓW�J�����
        */
            /*
                Input�\���� (Vertex�V�F�[�_����o�͂��ꂽ�l)
                    �Euv_MainTex //�e�N�X�`����UV���W
                    �EviewDir    //��������
                    �EworldPos   //���[���h���W
                    �EscreenPos  //�X�N���[�����W

                input ScurfaceOutputStanderd�\���� (�I�u�W�F�N�g�̕\�ʐF)
                    �Efixed3 Albedo      //��{�F
                    �Efixed3 Normal      //�@�����
                    �Ehalf3  Emission
                    �Ehalf   Metallic    //0 = �񃁃^�� / 1 = ���^��
                    �Ehalf   Smoothness  //0 = �e�� / 1 = ���炩
                    �Ehalf   Occlusion   //�I�N���[�W����(�f�t�H���g 1)
                    �Efixed  Alpha       //�����x�̃A���t�@
        */
        ENDCG
    }

    Fallback "Standard"
}
