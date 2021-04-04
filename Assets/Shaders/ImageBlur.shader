/*Shader "Custom/ImageBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Intensity ("Intensity of blur", Range(0, 10)) = 0
        _ImportanceDecrease ("Importance decrease", Range(0.001, 1)) = 1
        _DefaultColorUsage ("Default pixel color usage", Range(1, 200)) = 1
    }
    SubShader
    {
        Tags {"Queqe" = "Backgroud"}
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            float _Intensity;
            float _ImportanceDecrease;
            int _DefaultColorUsage;
            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                // fixed4 col = tex2D(_MainTex, i.texcoord);
                half4 color = half4(0.0h, 0.0h, 0.0h, 0.0h);
                float power = _Intensity / 2000;
                half count = 0;

                color += tex2D(_MainTex, i.texcoord) * _DefaultColorUsage;
                count += _DefaultColorUsage;
                
                if (_Intensity > 0) {
                    for (int it = 0; it < 5; it++) {
                        for (int it2 = 0; it2 < 5; it2++) {
                            float importance = 1 - max(it, it2) / (10 * _ImportanceDecrease);
                            if (importance < 0) continue;
                            color += tex2D(_MainTex, float2(i.texcoord.x + float(it) * power, i.texcoord.y + float(it2) * power)) * importance;
                            count += 1 * importance;
                            if (it != 0 && it2 != 0) {
                                color += tex2D(_MainTex, float2(i.texcoord.x + float(it) * power, i.texcoord.y - float(it2) * power)) * importance;
                                color += tex2D(_MainTex, float2(i.texcoord.x - float(it) * power, i.texcoord.y - float(it2) * power)) * importance;
                                color += tex2D(_MainTex, float2(i.texcoord.x - float(it) * power, i.texcoord.y + float(it2) * power)) * importance;
                                count += 3 * importance;
                            } else if (it2 == 0 && it != 0) {
                                color += tex2D(_MainTex, float2(i.texcoord.x - float(it) * power, i.texcoord.y)) * importance;
                                count += 1 * importance;
                            } else if (it == 0 && it2 != 0) {
                                color += tex2D(_MainTex, float2(i.texcoord.x, i.texcoord.y - float(it2) * power)) * importance;
                                count += 1 * importance;
                            }
                        }
                    }
                }

                color /= count;

                return color;
            }
            ENDCG
        }
    }
}*/
// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/ImageBlur"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

        _Intensity ("Intensity of blur", Range(0, 2)) = 0
        _ImportanceDecrease ("Importance decrease", Range(0.01, 1.5)) = 1 // range(0.0001, 1)
        _DefaultColorUsage ("Default pixel color usage", Range(1, 200)) = 1
        _ForceBlack ("Force black color on pixels", Range(0, 1)) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                OUT.color = v.color * _Color;
                return OUT;
            }

            
            float _Intensity;
            float _ImportanceDecrease;
            int _DefaultColorUsage;
            float _ForceBlack;
            
            static const half2 _valueMult[8] = {
                half2(0.95h, 0.3h),
                half2(0.89h, 0.45h),
                half2(0.8h, 0.6h),
                half2(0.71h, 0.71h),
                half2(0.71h, 0.71h),
                half2(0.8h, 0.6h),
                half2(0.89h, 0.45h),
                half2(0.95h, 0.3h)
            };

            fixed4 frag (v2f i) : SV_Target
            {
                // fixed4 col = tex2D(_MainTex, i.texcoord);
                half4 color = half4(0.0h, 0.0h, 0.0h, 0.0h);
                float power = _Intensity / 200;
                half count = 0.0h;

                color += tex2D(_MainTex, i.texcoord) * _DefaultColorUsage;
                count += _DefaultColorUsage;
                
                if (_Intensity > 0) {
                    float importance = _ImportanceDecrease;
                    
                #if HIGHER_RESOLUTION_SHADOW_ON_THEMES
                    color += tex2D(_MainTex, float2(i.texcoord.x + power * _valueMult[3].x, i.texcoord.y + power * _valueMult[3].y)) * importance;
                    color += tex2D(_MainTex, float2(i.texcoord.x + power * _valueMult[4].x, i.texcoord.y + power * _valueMult[4].y)) * importance;

                    color += tex2D(_MainTex, float2(i.texcoord.x - power * _valueMult[3].x, i.texcoord.y + power * _valueMult[3].y)) * importance;
                    color += tex2D(_MainTex, float2(i.texcoord.x - power * _valueMult[4].x, i.texcoord.y + power * _valueMult[4].y)) * importance;

                    color += tex2D(_MainTex, float2(i.texcoord.x - power * _valueMult[3].x, i.texcoord.y - power * _valueMult[3].y)) * importance;
                    color += tex2D(_MainTex, float2(i.texcoord.x - power * _valueMult[4].x, i.texcoord.y - power * _valueMult[4].y)) * importance;

                    color += tex2D(_MainTex, float2(i.texcoord.x + power * _valueMult[3].x, i.texcoord.y - power * _valueMult[3].y)) * importance;
                    color += tex2D(_MainTex, float2(i.texcoord.x + power * _valueMult[4].x, i.texcoord.y - power * _valueMult[4].y)) * importance;

                    count += importance * 2 * 4;
                #endif

                    color += tex2D(_MainTex, float2(i.texcoord.x + power * _valueMult[0].x, i.texcoord.y + power * _valueMult[0].y)) * importance;
                    // color += tex2D(_MainTex, float2(i.texcoord.x + power * _valueMult[1].x, i.texcoord.y + power * _valueMult[1].y)) * importance;
                    // color += tex2D(_MainTex, float2(i.texcoord.x + power * _valueMult[2].x, i.texcoord.y + power * _valueMult[2].y)) * importance;
                    // color += tex2D(_MainTex, float2(i.texcoord.x + power * _valueMult[5].x, i.texcoord.y + power * _valueMult[5].y)) * importance;
                    // color += tex2D(_MainTex, float2(i.texcoord.x + power * _valueMult[6].x, i.texcoord.y + power * _valueMult[6].y)) * importance;
                    color += tex2D(_MainTex, float2(i.texcoord.x + power * _valueMult[7].x, i.texcoord.y + power * _valueMult[7].y)) * importance;
                    
                    color += tex2D(_MainTex, float2(i.texcoord.x - power * _valueMult[0].x, i.texcoord.y + power * _valueMult[0].y)) * importance;
                    // color += tex2D(_MainTex, float2(i.texcoord.x - power * _valueMult[1].x, i.texcoord.y + power * _valueMult[1].y)) * importance;
                    // color += tex2D(_MainTex, float2(i.texcoord.x - power * _valueMult[2].x, i.texcoord.y + power * _valueMult[2].y)) * importance;
                    // color += tex2D(_MainTex, float2(i.texcoord.x - power * _valueMult[5].x, i.texcoord.y + power * _valueMult[5].y)) * importance;
                    // color += tex2D(_MainTex, float2(i.texcoord.x - power * _valueMult[6].x, i.texcoord.y + power * _valueMult[6].y)) * importance;
                    color += tex2D(_MainTex, float2(i.texcoord.x - power * _valueMult[7].x, i.texcoord.y + power * _valueMult[7].y)) * importance;
                    
                    color += tex2D(_MainTex, float2(i.texcoord.x - power * _valueMult[0].x, i.texcoord.y - power * _valueMult[0].y)) * importance;
                    // color += tex2D(_MainTex, float2(i.texcoord.x - power * _valueMult[1].x, i.texcoord.y - power * _valueMult[1].y)) * importance;
                    // color += tex2D(_MainTex, float2(i.texcoord.x - power * _valueMult[2].x, i.texcoord.y - power * _valueMult[2].y)) * importance;
                    // color += tex2D(_MainTex, float2(i.texcoord.x - power * _valueMult[5].x, i.texcoord.y - power * _valueMult[5].y)) * importance;
                    // color += tex2D(_MainTex, float2(i.texcoord.x - power * _valueMult[6].x, i.texcoord.y - power * _valueMult[6].y)) * importance;
                    color += tex2D(_MainTex, float2(i.texcoord.x - power * _valueMult[7].x, i.texcoord.y - power * _valueMult[7].y)) * importance;
                    
                    color += tex2D(_MainTex, float2(i.texcoord.x + power * _valueMult[0].x, i.texcoord.y - power * _valueMult[0].y)) * importance;
                    // color += tex2D(_MainTex, float2(i.texcoord.x + power * _valueMult[1].x, i.texcoord.y - power * _valueMult[1].y)) * importance;
                    // color += tex2D(_MainTex, float2(i.texcoord.x + power * _valueMult[2].x, i.texcoord.y - power * _valueMult[2].y)) * importance;
                    // color += tex2D(_MainTex, float2(i.texcoord.x + power * _valueMult[5].x, i.texcoord.y - power * _valueMult[5].y)) * importance;
                    // color += tex2D(_MainTex, float2(i.texcoord.x + power * _valueMult[6].x, i.texcoord.y - power * _valueMult[6].y)) * importance;
                    color += tex2D(_MainTex, float2(i.texcoord.x + power * _valueMult[7].x, i.texcoord.y - power * _valueMult[7].y)) * importance;
                    
                    count += importance * 2 * 4;
                    // for (int itDistance = 1; itDistance < 10; itDistance++) {
                    //     fixed importance = 1.0 - (float(itDistance) / float(10)) * pow(_ImportanceDecrease, 2);
                    //     if (importance < 0) break;

                    //     // some on plus sign, other on cross sign
                    //     if (itDistance % 2) {
                    //         float distMulPower = float(itDistance) * power;
                    //         color += tex2D(_MainTex, float2(i.texcoord.x + float(itDistance) * power, i.texcoord.y)) * importance;
                    //         color += tex2D(_MainTex, float2(i.texcoord.x - float(itDistance) * power, i.texcoord.y)) * importance;
                    //         color += tex2D(_MainTex, float2(i.texcoord.x, i.texcoord.y - float(itDistance) * power)) * importance;
                    //         color += tex2D(_MainTex, float2(i.texcoord.x, i.texcoord.y + float(itDistance) * power)) * importance;
                    //         count += importance * 4.0h;
                    //     } else {
                    //         color += tex2D(_MainTex, float2(i.texcoord.x + float(itDistance - 1) * power, i.texcoord.y + float(itDistance - 1) * power)) * importance;
                    //         color += tex2D(_MainTex, float2(i.texcoord.x - float(itDistance - 1) * power, i.texcoord.y + float(itDistance - 1) * power)) * importance;
                    //         color += tex2D(_MainTex, float2(i.texcoord.x - float(itDistance - 1) * power, i.texcoord.y - float(itDistance - 1) * power)) * importance;
                    //         color += tex2D(_MainTex, float2(i.texcoord.x + float(itDistance - 1) * power, i.texcoord.y - float(itDistance - 1) * power)) * importance;
                    //         count += importance * 4.0h;
                    //     }
                    // }


                    // for (int it = 0; it < 5; it++) {
                    //     for (int it2 = 0; it2 < 5; it2++) {
                    //         float importance = 1 - max(it, it2) / (10 * _ImportanceDecrease);
                    //         if (importance < 0) continue;
                    //         color += tex2D(_MainTex, float2(i.texcoord.x + float(it) * power, i.texcoord.y + float(it2) * power)) * importance;
                    //         count += 1 * importance;
                    //         if (it != 0 && it2 != 0) {
                    //             color += tex2D(_MainTex, float2(i.texcoord.x + float(it) * power, i.texcoord.y - float(it2) * power)) * importance;
                    //             color += tex2D(_MainTex, float2(i.texcoord.x - float(it) * power, i.texcoord.y - float(it2) * power)) * importance;
                    //             color += tex2D(_MainTex, float2(i.texcoord.x - float(it) * power, i.texcoord.y + float(it2) * power)) * importance;
                    //             count += 3 * importance;
                    //         } else if (it2 == 0 && it != 0) {
                    //             color += tex2D(_MainTex, float2(i.texcoord.x - float(it) * power, i.texcoord.y)) * importance;
                    //             count += 1 * importance;
                    //         } else if (it == 0 && it2 != 0) {
                    //             color += tex2D(_MainTex, float2(i.texcoord.x, i.texcoord.y - float(it2) * power)) * importance;
                    //             count += 1 * importance;
                    //         }
                    //     }
                    // }
                }

                color /= count;
                color *= i.color;
                color = float4(color.rgb - color.rgb * _ForceBlack, color.a);

                return color;
            }
        ENDCG
        }
    }
}