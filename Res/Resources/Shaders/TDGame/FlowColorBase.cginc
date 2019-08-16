#ifndef FlowColorBase_INCLUDE
	#define FlowColorBase_INCLUDE 
	#include "UnityCG.cginc"

	#if UseFlowColor
		uniform sampler2D _FlowTex;
		uniform half4 _FlowTex_ST;
		uniform half4 _FlowColor;
		uniform float _FlowSpeed;
	#endif
	
	#define FLOW_VERT(o) \
		o.flowUv = TRANSFORM_TEX(v.texcoord, _FlowTex); \
		o.flowUv.x += _Time.y * _FlowSpeed
	
	#define FLOW_FRAG(col) \
		fixed4 flowCol = tex2D(_FlowTex, i.flowUv); \
		flowCol *= _FlowColor; \
		col.rgb += flowCol.rgb * flowCol.a
	
	#define FLOW_UV(idx) \
		float2 flowUv : TEXCOORD##idx;
#endif