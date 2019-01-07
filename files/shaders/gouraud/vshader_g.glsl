// VERTEX SHADER. GOURAUD

#version 330

in vec3 vNormal;
in vec3 vPos;

uniform mat4 projMat;
uniform mat4 mvMat;
uniform mat3 mnMat;

// Posicion de la luz
uniform vec3 lPos;

//coeficientes que representan el material
uniform vec4 ka;
uniform vec4 kd;
uniform vec4 ks;
uniform float CoefEsp;

out vec4 fragColor;

void main(){

	// transformar posición de los vert de entrada del Espacio Obj al EClipping
	gl_Position = projMat * mvMat * vec4(vPos, 1.0);

	// transf posición de los vértices de entrada del Esp.obj al Esp.ojo (vE)
	vec4 vE = mvMat * vec4(vPos, 1.0);

	// Calc. vector luz en Espacio del ojo
	vec3 vLE = lPos - vE.xyz;
	vec3 L = normalize(vLE);

	// Vector del vertice al ojo en Espacio del ojo
	vec3 V = normalize(-vE.xyz);

	vec3 N = normalize(mnMat * vNormal);
	vec3 R = normalize(reflect(-L,N));

	// Calc térm difuso+espec de Phong
	float difuso = max(dot(L,N), 0.0);
	float specPhong = pow(max(dot(R, V), 0.0), CoefEsp);

	if (dot(L,N) < 0.0) {
		specPhong = 0.0;
	}

	fragColor = ka + kd * difuso + ks * specPhong;
}

