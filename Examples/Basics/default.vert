#version 330

layout(location = 0) in vec4 position;
layout(location = 1) in vec4 colour;

smooth out vec4 diffuseColor;

uniform mat4 _model = mat4(1.0);
uniform mat4 _projection = mat4(1.0);

void main()
{	
	gl_Position = position * _model * _projection;      
	
	diffuseColor = colour;
}