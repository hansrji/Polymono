#version 400 core

out vec4 FragColor;

uniform vec4 colour;
uniform float time;

void main()
{
	FragColor = colour;
}