Shader "Unlit/Lava Distort" 
{
	Properties{
		//_Color ("Main Color", Color) = (1,1,1)
		_DistortX("Distortion in X", Range(0,2)) = 1
		_DistortY("Distortion in Y", Range(0,2)) = 0
		_MainTex("_MainTex RGBA", 2D) = "white" {}
		_Distort("_Distort A", 2D) = "white" {}
		_LavaTex("_LavaTex RGB", 2D) = "white" {}
		_BumpMap("Normalmap", 2D) = "bump" {}
	}

	SubShader{
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Opaque" }

		GrabPass{ "_GrabTexture" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _GrabTexture;
			sampler2D _MainTex;
			sampler2D _Distort;
			sampler2D _LavaTex;
			sampler2D _BumpMap;

			fixed4 _MainTex_ST;
			fixed4 _Distort_ST;
			fixed4 _LavaTex_ST;
			fixed4 _Color;
			fixed _DistortX;
			fixed _DistortY;

			struct appdata_t {
				fixed4 vertex : POSITION;
				fixed4 color : COLOR;
				fixed2 texcoord : TEXCOORD0;
				fixed2 texcoord1 : TEXCOORD1;
			};

			struct v2f {
				fixed4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				fixed2 texcoord : TEXCOORD0;
				fixed2 texcoord1 : TEXCOORD1;
				fixed4 uvgrab : TEXCOORD2;
			};

			v2f vert(appdata_t v) {
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				o.texcoord1 = TRANSFORM_TEX(v.texcoord1,_LavaTex);
				o.uvgrab = ComputeGrabScreenPos(o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : Color {
				fixed4 tex = tex2D(_MainTex, i.texcoord);
				fixed distort = tex2D(_Distort, i.texcoord).a;
				fixed4 tex2 = tex2D(_LavaTex, fixed2(i.texcoord1.x - distort*_DistortX,i.texcoord1.y - distort*_DistortY));
				
				fixed4 bump = tex2D(_BumpMap, i.texcoord);
				fixed2 distortion = UnpackNormal(bump).rg;

				i.uvgrab.xy += distortion;

				fixed4 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));

				tex = tex*tex.a*col + tex2*(1 - tex.a);

				
				return tex;
			}
			ENDCG
		}
	}
}