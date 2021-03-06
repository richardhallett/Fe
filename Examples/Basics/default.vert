﻿// Default Vert
#version 330

layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;
layout(location = 3) in vec4 colour;

smooth out vec4 diffuseColor;

uniform mat4 _model = mat4(1.0);
uniform mat4 _projection = mat4(1.0);

void main()
{	
	gl_Position = vec4(position, 1.0f) * _model * _projection;      
	
	diffuseColor = colour;
}