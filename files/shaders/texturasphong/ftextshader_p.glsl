// FRAGMENT SHADER.
#version 330

struct Material {
	vec3 Ka;
	vec3 Kd;
	vec3 Ks;
	float shininess;
};
struct Light {
	vec4 position;
	vec3 Ia;
	vec3 Id;
	vec3 Is;
	float coneAngle;
	vec3 coneDirection;
	int enabled;
};

in vec3 fragPos, fragNormal;
in vec2 f_TexCoord;


uniform mat4 modelMatrix;
uniform mat3 normalMatrix;
uniform vec3 cameraPosition; //In World Space.
uniform Light myLight;
uniform Material material;
uniform float A, B, C;
uniform float tim;

uniform sampler2D gSampler; 

out vec4 fragColor;

vec4 applyLight(Light light, Material material, vec3 surfacePos, vec3 surfaceNormal, vec3 surfaceToCamera) {
	float attenuation = 1;
	vec3 surfaceToLight;
	if (light.position.w == 0.0) { //Directional light
		surfaceToLight = normalize(-light.position.xyz);
		attenuation = 1.0; //no attenuation for directional lights.
	} else { //Positional light (Spot or Point)
		surfaceToLight = normalize(light.position.xyz - surfacePos);
		//Cone restrictions
		vec3 coneDirection = normalize(light.coneDirection);
		vec3 rayDirection = -surfaceToLight;
		float lightToSurfaceAngle = degrees(acos(dot(rayDirection, coneDirection)));
		
		
		float distanceToLight = length(light.position.xyz - surfacePos);
		
		attenuation = (1.0 - tim / 1000) / ( A + B * distanceToLight + C * pow(distanceToLight, 2));
		
	}
	//AMBIENT
	vec4 ambient = vec4(light.Ia, 1.0) * vec4(material.Ka, 1);

	//DIFUSSE
	float diffuseCoefficient = max(0.0, dot(surfaceNormal, surfaceToLight));
	vec4 diffuse = vec4(light.Id,1.0) * vec4(material.Kd, 1) * diffuseCoefficient * (texture2D(gSampler, f_TexCoord.st));

	//SPECULAR
	float specularCoefficient = 0.0;
	if (diffuseCoefficient > 0.0) {
		vec3 incidenceVector = -surfaceToLight;
		vec3 reflectionVector = reflect(incidenceVector, surfaceNormal);
		float cosAngle = max(0.0, dot(surfaceToCamera, reflectionVector));
		specularCoefficient = pow(cosAngle, material.shininess);
	}
	vec4 specular = vec4(light.Is, 1.0) * vec4(material.Ks, 1) * specularCoefficient;
	return ambient + attenuation * (diffuse + specular) * light.enabled;
}

void main() {
	vec3 surfacePos = vec3(modelMatrix * vec4(fragPos, 1));
	vec3 surfaceNormal = normalize(normalMatrix * fragNormal);
	vec3 surfaceToCamera = normalize(cameraPosition - surfacePos);
	
	vec4 linearColor = applyLight(myLight, material, surfacePos, surfaceNormal, surfaceToCamera);

	fragColor = linearColor;
}

