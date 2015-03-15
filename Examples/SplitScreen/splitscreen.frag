#version 330

smooth in vec4 diffuseColor;
out vec4 outputColor;

void main()
{
   outputColor = diffuseColor;
}