#version 400 core

in vec3 vPosition;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position = vec4(vPosition.xyz, 1.0) * model * view * projection;
}