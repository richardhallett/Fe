#version 330

layout(location = 0) in vec4 position;
layout(location = 1) in vec4 colour;

smooth out vec4 diffuseColor;

uniform mat4 _model;

uniform mat4 projectionMatrix;

void main()
{	
	gl_Position = position * _model * projectionMatrix;      
	
	diffuseColor = colour;
}