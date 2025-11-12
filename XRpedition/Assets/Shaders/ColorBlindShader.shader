Shader "Custom/ColorBlindnessSim"
{
    Properties
    {
        _MainTex ("Passthrough Texture", 2D) = "white" {}
        _Mode ("Mode", Int) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            int _Mode;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

            float3 ApplyCVD(float3 c, int mode)
            {
                if (mode == 1) // Deuteranopia
                {
                    float3x3 m = float3x3(
                        0.625, 0.375, 0.0,
                        0.7,   0.3,   0.0,
                        0.0,   0.3,   0.7
                    );
                    c = mul(m, c);
                }
                else if (mode == 2) // Protanopia
                {
                    float3x3 m = float3x3(
                        0.567, 0.433, 0.0,
                        0.558, 0.442, 0.0,
                        0.0,   0.242, 0.758
                    );
                    c = mul(m, c);
                }
                else if (mode == 3) // Tritanopia
                {
                    float3x3 m = float3x3(
                        0.95,  0.05,  0.0,
                        0.0,   0.433, 0.567,
                        0.0,   0.475, 0.525
                    );
                    c = mul(m, c);
                }
                else if (mode == 4) //Black & white (GreyScale)
                {
                    float luminace = dot(c, float3(0.299,0.587,0.114));
                    c = float3(luminace,luminace,luminace);
                }
                return saturate(c);
            }

            half4 frag (Varyings i) : SV_Target
            {
                float3 col = tex2D(_MainTex, i.uv).rgb;
                col = ApplyCVD(col, _Mode);
                return half4(col, 1.0);
            }
            ENDHLSL
        }
    }
}