#version 400 core

in vec3 vPosition;
in vec2 vTexture;
in vec3 vNormal;
in vec4 vColour;

out vec2 fTexture;
out vec4 fColour;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position = vec4(vPosition.xyz, 1.0) * model * view * projection;
    fTexture = vTexture;
    fColour = vColour;
}