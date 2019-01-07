// VERTEX SHADER. 

#version 330
in vec3 vPos;
in vec3 vNormal;
in vec2 TexCoord;

out vec2 Vertex_UV;
out vec4 Vertex_Normal;
out vec4 Vertex_LightDir;
out vec4 Vertex_EyeVec;
 
// Automatically passed by GLSL Hacker
uniform mat4 gxl3d_ModelViewProjectionMatrix;
 
// Automatically passed by GLSL Hacker
uniform mat4 projMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;
 
uniform vec4 lPos;
 
void main()
{
  gl_Position = projMatrix * viewMatrix * modelMatrix * vec4(vPos, 1.0);
  Vertex_UV = TexCoord;
  Vertex_Normal = viewMatrix * modelMatrix  * vec4(vNormal, 1.0);
  vec4 view_vertex = viewMatrix * modelMatrix  * vec4(vPos, 1.0);
  Vertex_LightDir = lPos - view_vertex;
  Vertex_EyeVec = view_vertex;
}