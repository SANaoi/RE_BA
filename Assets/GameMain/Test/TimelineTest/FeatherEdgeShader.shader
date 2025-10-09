Shader "Custom/FeatherEdge_Directional" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Feather ("Feather Size", Range(0, 1)) = 0.1 // 羽化范围（0~0.5，值越大羽化越宽）
        _Color ("Tint", Color) = (1, 1, 1, 1)
    }
    SubShader {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off ZWrite Off

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Feather;
            float4 _Color;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                // 计算“上方”的距离：UV的y轴范围是[0,1]，顶部为1，底部为0
                // 只针对上方区域计算羽化（y值越大，越靠近顶部）
                float topDist = i.uv.y; // 顶部距离：0（底部）~1（顶部）

                // 羽化范围：只在顶部的_Feather区域内渐变
                // 例如：_Feather=0.1时，y在0.9~1.0之间的区域会被羽化
                float featherFactor = smoothstep(1 - _Feather, 1, topDist);

                // 应用羽化：顶部边缘透明度降低，其他区域不受影响
                col.a *= (1 - featherFactor);

                return col;
            }
            ENDCG
        }
    }
}