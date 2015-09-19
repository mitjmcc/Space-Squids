Shader "Custom/SkyBoxReflectionWithRim" {
	Properties {
		_MainTex("Texture", 2D) = "white" {}
		_BumpMap("Bumpmap", 2D) = "bump" {}
		_Cube("Cubemap", CUBE) = "" {}
		_RimColor("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimPower("Rim Power", Range(0.5,8.0)) = 3.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		#pragma surface surf Lambert
		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 worldRefl;
			float3 viewDir;
			INTERNAL_DATA
		};

		sampler2D _MainTex;
		sampler2D _BumpMap;
		samplerCUBE _Cube;
		float4 _RimColor;
		float _RimPower;

		void surf(Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * 0.5;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			half rim = saturate(dot(normalize(IN.viewDir), o.Normal));
			o.Emission = _RimColor.rgb * pow(rim, _RimPower) * texCUBE(_Cube, WorldReflectionVector(IN, o.Normal)).rgb;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}