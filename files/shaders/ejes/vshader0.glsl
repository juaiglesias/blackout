// VERTEX SHADER. Simple

#version 330

uniform mat4 projMat;
uniform mat4 mMat;
uniform mat4 vMat;

in vec3 vPos;

void main(){
  gl_Position = projMat * vMat * mMat * vec4(vPos, 1.0);
}
