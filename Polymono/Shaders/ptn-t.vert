#version 400 core

in vec3 vPosition;
in vec2 vTexture;
in vec3 vNormal;

out vec2 fTexture;
out vec3 fNormal;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position = vec4(vPosition.xyz, 1.0) * model * view * projection;
    fTexture = vTexture;
}