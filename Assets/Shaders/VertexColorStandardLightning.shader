Shader "Custom/VertexColorStandardLightning" {
	Properties {
		_ColorMat("Color", Color) = (1, 1, 1, 1)
		_Metallic("Metallic", Range(0,1)) = 0
		_Smoothness("Smothness", Range(0, 1)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard vertex:vert
		#pragma target 3.0

		struct Input {
			float4 vertColor;
		};

		float4 _ColorMat;
		half _Metallic;
		half _Smoothness;

		void vert(inout appdata_full v, out Input o){
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.vertColor = v.color * _ColorMat;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = IN.vertColor.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
		}
		ENDCG
	}
	FallBack "Diffuse"
}