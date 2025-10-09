Shader "Custom/EyeMouthShader_Fixed"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _MouthTex("Mouth Texture", 2D) = "white" {}
        _FaceTex("Face Texture", 2D) = "white" {}
        _MouthBlend("Mouth Blend Factor", Range(0, 1)) = 1.0
        _offsetX("X offset", float) = 0
        _offsetY("Y offset", float) = 0
        _MouthRegion("Mouth Region", Vector) = (0.0, 0.25, 0.0, 0.25) // xMin, xMax, yMin, yMax
        _AlphaThreshold("Alpha Threshold", Range(0, 1)) = 0.01
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }
        LOD 200

        Pass
        {
            ZWrite On
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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _MouthTex;
            sampler2D _FaceTex;
            float _MouthBlend;
            float _offsetX;
            float _offsetY;
            float4 _MouthRegion;
            float _AlphaThreshold;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 mainColor = tex2D(_MainTex, i.uv);
                fixed4 faceColor = tex2D(_FaceTex, i.uv);
                
                // 计算嘴部UV
                float2 mouthUV = i.uv + float2(_offsetX, _offsetY);
                fixed4 mouthColor = tex2D(_MouthTex, mouthUV);
                
                // 检查是否在嘴部区域内
                bool inMouthRegion = i.uv.x >= _MouthRegion.x && i.uv.x <= _MouthRegion.y &&
                                   i.uv.y >= _MouthRegion.z && i.uv.y <= _MouthRegion.w;
                
                if (inMouthRegion)
                {
                    // 方法1：使用Alpha阈值来避免边缘白色像素
                    if (mouthColor.a > _AlphaThreshold)
                    {
                        // 预乘Alpha混合，避免边缘白边
                        mouthColor.rgb *= mouthColor.a;
                        mainColor.rgb = lerp(mainColor.rgb, mouthColor.rgb, _MouthBlend * mouthColor.a);
                    }
                    else
                    {
                        // 如果嘴部纹理Alpha很低，使用脸部纹理
                        mainColor.rgb = lerp(mainColor.rgb, faceColor.rgb, _MouthBlend * faceColor.a);
                    }
                }
                
                return mainColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}