#version 330

in vec2 UV;
uniform vec4 colour = vec4(1.0, 1.0, 1.0, 1.0);

uniform sampler2D colourMap;

smooth in vec4 diffuseColor;
out vec4 outputColor;

void main()
{
   //outputColor = colour;
   outputColor = texture( colourMap, UV );
}