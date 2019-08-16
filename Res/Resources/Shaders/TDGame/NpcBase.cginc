#ifndef NpcBase_INCLUDE
	#define NpcBase_INCLUDE
	#include "UnityCG.cginc"
	#include "Gles2Helper.cginc"
	#include "FlowColorBase.cginc"

	uniform sampler2D _MainTex;
	uniform float4 _MainTex_ST;
	uniform half _Emission;
	uniform half4 _Color;
	uniform half4 _AddColor;
	uniform half _Outline;
	uniform half4 _OutlineColor;

	// 注：边缘光是优化算法，无法线贴图时，切线空间法线为（0,0,1），点乘结果为viewDir的z
	#define NPC_VERT_COMMON(o) \
		o.pos = UnityObjectToClipPos(v.vertex); \
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex); \
		half3 objectViewDir = ObjSpaceViewDir(v.vertex); \
		TANGENT_SPACE_ROTATION; \
		o.rim = saturate(1 - normalize(MUL_3x3_WITH_VECTOR(rotation, objectViewDir)).z); \
		o.rim = pow(o.rim, _Outline);
	
	#define NPC_FRAG_COMMON(col) \
		col = tex2D(_MainTex, i.texcoord); \
		col.rgb *= _Color.rgb; \
		col.rgb += col.rgb * _Emission + _AddColor; \
		col.rgb += _OutlineColor.rgb * _OutlineColor.a * i.rim
#endif