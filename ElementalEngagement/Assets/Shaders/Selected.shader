Shader "Custom/DrawCircle"
{
	Properties
	{
		_OutlineColor("Outline Color", Color) = (1,0,0,1)
		_Color("Base Color", Color) = (0,1,0,1)
		_OutlineBoundary("Outline Boundary" , Range(0, 1)) = 0.2
	}
		SubShader
	{
		Pass
		{
		// Setup our pass to use Forward rendering, and only receive
		// data on the main directional light and ambient light.
		Tags {	}

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float4 uv : TEXCOORD0;
			float3 normal : NORMAL;
		};

		struct v2f
		{
			float4 pos : SV_POSITION;
			float3 worldNormal : NORMAL;
			float2 uv : TEXCOORD0;
			float3 viewDir : TEXCOORD1;
		};

		sampler2D _MainTex;
		float4 _MainTex_ST;

		v2f vert(appdata v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.worldNormal = UnityObjectToWorldNormal(v.normal);
			o.viewDir = WorldSpaceViewDir(v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			return o;
		}

		float4 _Color;
		float4 _OutlineColor;
		float _OutlineBoundary;

		float4 frag(v2f i) : SV_Target
		{
			float3 normal = normalize(i.worldNormal);
			float3 viewDir = normalize(i.viewDir);

			// Calculate outline lighting
			float outline = dot(viewDir, normal);
			if (outline <= _OutlineBoundary)
				return _OutlineColor;
			return _Color;
		}
		ENDCG
	}
	}
		FallBack "Diffuse"
}
