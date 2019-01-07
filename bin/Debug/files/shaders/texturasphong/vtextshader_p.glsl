// VERTEX SHADER. Simple

#version 330
uniform mat4 projMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;

in vec3 vPos;
in vec3 vNormal;
in vec2 TexCoord;

out vec3 fragPos;
out vec3 fragNormal;
out vec2 f_TexCoord;

void main(){
 fragPos = vPos;
 fragNormal = vNormal;
  f_TexCoord = TexCoord;

 gl_Position = projMatrix * viewMatrix * modelMatrix * vec4(vPos, 1.0);
}
