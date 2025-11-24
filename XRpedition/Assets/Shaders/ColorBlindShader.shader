Shader "Custom/LUTMaterialShader"
{
    Properties
    {
        _BaseMap ("Base Map", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _LutTex ("LUT Texture", 2D) = "white" {}
        _Contribution ("LUT Strength", Range(0,1)) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // URP shader includes
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Textures & uniforms
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            TEXTURE2D(_LutTex);
            SAMPLER(sampler_LutTex);

            float4 _Color;
            float _Contribution;

            struct Attributes
            {
                float3 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // Vertex program — converts object space → clip space
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            // Function to apply the LUT texture
            float4 ApplyLUT(float4 color)
            {
                // LUT is 16 slices vertically
                float sliceCount = 16.0;

                float slice = color.b * (sliceCount - 1.0);
                float sliceFloor = floor(slice);
                float sliceFrac = slice - sliceFloor;

                float sliceSize = 1.0 / sliceCount;

                float2 uv1 = float2(
                    color.r,
                    sliceFloor * sliceSize + color.g * sliceSize
                );

                float2 uv2 = float2(
                    color.r,
                    (sliceFloor + 1.0) * sliceSize + color.g * sliceSize
                );

                float4 c1 = SAMPLE_TEXTURE2D(_LutTex, sampler_LutTex, uv1);
                float4 c2 = SAMPLE_TEXTURE2D(_LutTex, sampler_LutTex, uv2);

                return lerp(c1, c2, sliceFrac);
            }

            // Fragment program — applies LUT
            float4 frag(Varyings IN) : SV_Target
            {
                float4 baseCol = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _Color;
                float4 lutCol = ApplyLUT(baseCol);

                return lerp(baseCol, lutCol, _Contribution);
            }

            ENDHLSL
        }
    }

    FallBack Off
}
