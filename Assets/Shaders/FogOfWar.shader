Shader "Unlit/FogOfWarArray"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1.0,1.0,1.0,1.0)
        _FogOfWarBreakersArraySize("Array Size", int) = 16
        _FadeOutDistance("Fade Distance", float) = 2
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "Render" = "Transparent" "IgnoreProjector" = "True" }
        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha 

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD1;
            };
            
            uniform float4 _FogOfWarBreakers[16];
            int _FogOfWarBreakersArraySize;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _FadeOutDistance;

            v2f vert (appdata v)
            {
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float invLerp(float from, float to, float value) {
                return clamp((value - from) / (to - from), 0, 1);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);// ;
                col.a = 1;
                for (int j = 0; j < _FogOfWarBreakersArraySize; j++)
                {
                    float dist = distance(_FogOfWarBreakers[j].xz, i.worldPos.xz);
                    float unitRange = _FogOfWarBreakers[j].y;
                    if (dist <= unitRange)
                    {
                        if(dist < unitRange && dist > unitRange - _FadeOutDistance)
                        {
                            col.a = min(col.a, invLerp(unitRange - _FadeOutDistance, unitRange, dist));
                        }else if(dist < unitRange)
                        {
                            col.a = min(col.a, 0);
                        }
                    }
                }

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}