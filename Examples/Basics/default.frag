#version 330

uniform vec4 colour = vec4(1.0, 1.0, 1.0, 1.0);

smooth in vec4 diffuseColor;
out vec4 outputColor;

void main()
{
   outputColor = colour;
}