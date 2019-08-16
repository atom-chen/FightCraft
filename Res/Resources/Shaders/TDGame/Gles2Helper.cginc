// ����Gles 2.0ƽ̨�£�һЩ������㷽����֧�ֵ����
#ifndef GLES2_HELPER_INCLUDE  
	#define GLES2_HELPER_INCLUDE

	#define MUL_3x3_WITH_VECTOR(matrix, vector) float3(dot(matrix[0], vector), dot(matrix[1], vector), dot(matrix[2], vector))
	
	#define MUL_VECTOR_WITH_3x3(vector, matrix) vector[0] * matrix[0] + vector[1] * matrix[1] + vector[2] * matrix[2]
#endif