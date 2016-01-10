#version 330

layout(location = 0) in vec4 position;

uniform mat4 _model;
uniform mat4 _projection;

void main()
{	
	gl_Position = position * _model * _projection;      	
}