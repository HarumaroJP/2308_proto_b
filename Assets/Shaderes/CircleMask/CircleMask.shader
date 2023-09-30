Shader "Unlit/CircleMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_radius ("Radius", float) = 1.0
		[Space]
		_circleOffsetX ("OffsetX", float) = 0.0
		_circleOffsetY ("OffsetY", float) = 0.0
		[Space]
		_circleScaleX ("CircleScaleX", float) = 1.0
		_circleScaleY ("CircleScaleY", float) = 1.0
		[Space]
		[Toggle] _invert("Invert", int) = 0

    }

    SubShader
    {
        Tags { "RenderType"="Overray" }
		Blend SrcAlpha OneMinusSrcAlpha 
        LOD 100

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
				float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float3 localPos : TEXCOORD1;
				float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 	_MainTex_ST;
			float 	_circleScaleX, _circleScaleY;
			float	_circleOffsetX, _circleOffsetY;;
			float  	_radius;
			int 	_invert;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

				o.localPos = v.vertex.xyz;
				o.localPos.x *= _circleScaleX;
				o.localPos.y *= _circleScaleY;
				o.localPos.xy -= float2(_circleOffsetX, _circleOffsetY);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                UNITY_TRANSFER_FOG(o,o.vertex);
				
				o.color = v.color;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				col *= i.color;

				float2 p = i.localPos.x * i.localPos.x + i.localPos.y * i.localPos.y;
				float inside = step(p, _radius * _radius);
				inside = lerp(inside, 1.0f - inside, _invert);

                return lerp((fixed4)0, col, inside);

				return (fixed4)i.localPos.x;
                // return fixed4(i.uv.x, i.uv.y, 0.0f, 1.0f);
            }
            ENDCG
        }
    }
}
