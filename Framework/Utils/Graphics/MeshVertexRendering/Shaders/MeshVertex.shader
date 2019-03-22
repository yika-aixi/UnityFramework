Shader "Mesh Vertex Rendering/Mesh Vertex Shader"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_VertexPositions ("Vertex Buffer", 2D) = "white" {}
		_VertexPositionsSize ("Vertex Buffer Size", Float) = 1.0
		_QuadSize ("Quad Size", Float) = 1.0
		[PerRendererData] _Color ("Color", Color) = (1,1,1,1)
	}
	
	CGINCLUDE
		#define _ALPHABLEND_ON
    ENDCG
	
	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
		LOD 100
		
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Lighting Off
			//ZTest Always
			ZWrite Off
			Cull Off
			Lighting Off
			
			CGPROGRAM			
				#pragma multi_compile_instancing
				
				#pragma vertex vert
				#pragma fragment frag
				
				#include "CGIncludes/MeshVertexBillboards.cginc"
			ENDCG
		}
	}
}
