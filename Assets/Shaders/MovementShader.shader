Shader "Unlit/MovementShader"
{
    Properties
    {
        _Color ("Tint", Color) = (0, 0, 0, 1)
        _MainTex ("Texture", 2D) = "white" {}        
        _MaskTexture ("Mask Texture", 2D) = "white" {}
        _Influence ("Mask Influence", Range(0.0, 1.0)) = 0
        
        _NormalMap("NormalMap", 2D) = "white" {}
        
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha

        ZWrite off
        Cull off

        Pass
        {

            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _MaskTexture;
            sampler2D _NormalMap;
            
            fixed4 _Color;
            float _Influence;

            float3 _LightDirection;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 viewPosition : TEXCOORD1;
                float3 tbn[3] : TEXCOORD2; // TEXCOORD2; TEXCOORD3;
                fixed4 color : COLOR;
            };


            float map(float s, float a1, float a2, float b1, float b2)
            {
                return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
            }
            
            v2f vert(appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                float4 worldPos = mul (unity_ObjectToWorld, v.vertex);
                o.position.y += _Influence * sin((_Time * 100) + worldPos.x) * 0.02f;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                o.viewPosition = UnityObjectToViewPos(v.vertex);


                float3 normal = UnityObjectToWorldNormal(v.normal);
				float3 tangent = UnityObjectToWorldNormal(v.tangent);
				float3 bitangent = cross(tangent, normal);

				o.tbn[0] = tangent;
				o.tbn[1] = bitangent;
				o.tbn[2] = normal;

                
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET
            {
                float3 tangentNormal = tex2D(_NormalMap, i.viewPosition.xy * 0.2) * 2 - 1;
				float3 surfaceNormal = i.tbn[2];
				float3 worldNormal = float3(i.tbn[0] * tangentNormal.r + i.tbn[1] * tangentNormal.g + i.tbn[2] * tangentNormal.b);

				float3 normalHighlights = saturate(dot(worldNormal, normalize(_LightDirection)));


                fixed4 mask = tex2D(_MaskTexture, i.viewPosition.xy * 0.2);

                float4 col = mask * _Color * i.color;

                col = col * i.color;


                return fixed4(col.rgb * normalHighlights, col.a);
                
            }
            ENDCG
        }
    }
}