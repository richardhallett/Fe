#version 330

in vec2 UV;

uniform sampler2D colourMap;

out vec4 outputColor;

void main()
{   
   outputColor = texture( colourMap, UV );
}