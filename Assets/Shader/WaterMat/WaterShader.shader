Shader "Custom/URP/TilemapWater"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _WaveStrength ("Wave Strength", Range(0, 0.4)) = 0.08
        _WaveSpeed ("Wave Speed", Float) = 2.0
        _WaveFrequency ("Wave Frequency", Float) = 7.0
        _FlowSpeed ("Flow Speed", Float) = 1.2
        _PixelSize ("Pixel Size", Range(1, 64)) = 16
        _FoamBrightness ("Foam Brightness", Range(1, 2)) = 1.15
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "UniversalMaterialType"="Unlit"
        }

        Pass
        {
            Name "Unlit"
            Tags { "LightMode"="Universal2D" }

            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float2 worldPos : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float _WaveStrength;
                float _WaveSpeed;
                float _WaveFrequency;
                float _FlowSpeed;
                float _PixelSize;
                float _FoamBrightness;
            CBUFFER_END

            Varyings Vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs posInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionHCS = posInputs.positionCS;
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color;
                OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz).xy;
                return OUT;
            }

            half4 Frag(Varyings IN) : SV_Target
            {
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * IN.color;

                float brightness = dot(col.rgb, float3(0.299, 0.587, 0.114));
                float foamMask = smoothstep(0.72, 0.95, brightness);

                float blueMask = saturate((col.b - max(col.r, col.g)) * 4.0);
                float waterMask = blueMask * (1.0 - foamMask) * col.a;

                float2 pixelWorld = floor(IN.worldPos * _PixelSize) / _PixelSize;
                float t = _Time.y;

                float waveA = sin(pixelWorld.x * _WaveFrequency - t * _FlowSpeed + sin(pixelWorld.y * 5.0 + t * _WaveSpeed) * 0.8);
                float waveB = sin((pixelWorld.x + pixelWorld.y * 0.35) * (_WaveFrequency * 0.55) - t * (_FlowSpeed * 0.7));
                float wave = (waveA * 0.65 + waveB * 0.35) * 0.5 + 0.5;

                half3 waterColor = col.rgb;
                waterColor *= 1.0 + ((wave - 0.5) * _WaveStrength * waterMask);
                waterColor += half3(0.03, 0.06, 0.10) * wave * _WaveStrength * waterMask;

                half3 finalRgb = lerp(waterColor, col.rgb * _FoamBrightness, foamMask);
                return half4(saturate(finalRgb), col.a);
            }
            ENDHLSL
        }
    }
}
