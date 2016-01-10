#version 330

uniform vec4 colour = vec4(1.0, 0.0, 0.0, 1.0);
out vec4 outputColor;

void main()
{
   outputColor = colour;
}