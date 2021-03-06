#version 400 core

in vec2 fTexture;

out vec4 FragColor;

uniform sampler2D texture0;

void main()
{
    FragColor = texture(texture0, fTexture);
}