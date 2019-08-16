Shader "Zhanyou/Character/NPC" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Emission ("Emission", Range(0, 1)) = 0.2
		// 角色材质颜色，乘法运算
		_Color("Blend Color", Color) = (1, 1, 1, 1)
		// Emission叠加颜色，加法运算
		_AddColor("Additive Color", Color) = (0, 0, 0, 1)
		_Outline ("Outline Power", float) = 22
		_OutlineColor ("Outline Color", Color) = (0.02, 0.02, 0.02, 1)
		// 流光贴图
		_FlowTex ("Flow (RGB)", 2D) = "black" {}
		// 流光颜色
		_FlowColor ("Flow Color", Color) = (1, 1, 1, 1)
		// 流光速度
		_FlowSpeed ("Flow Speed", float) = 1
	}

	SubShader {
		Tags { "Queue"="AlphaTest+10" "RenderType"="Character" }

		Pass {
            Stencil {
                Ref 1
                Comp always
                Pass replace
            }
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
            #pragma multi_compile __ UseFlowColor
			#pragma multi_compile_fog
			#include "NpcBase.cginc"
			
			struct v2f {
				float4 pos : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				half rim : TEXCOORD1;
				UNITY_FOG_COORDS(2)
				#if UseFlowColor
					FLOW_UV(3)
				#endif
			};
	
			v2f vert (appdata_tan v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				NPC_VERT_COMMON(o);
				#if UseFlowColor
					FLOW_VERT(o);
				#endif
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}
	
			fixed4 frag(v2f i) : SV_TARGET
			{
				fixed4 col;
				NPC_FRAG_COMMON(col);
				#if UseFlowColor
					FLOW_FRAG(col);
				#endif
				UNITY_OPAQUE_ALPHA(col.a);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
	CustomEditor "FlowToggleShaderGui"
	Fallback "Unlit/Texture"
}