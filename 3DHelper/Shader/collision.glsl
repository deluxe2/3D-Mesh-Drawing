#version 430

//obj1
vec3 pos1;
vec3 speed1;
float mass1;
float elasticity1;

//obj2
vec3 pos2;
vec3 speed2;
float mass2;
float elasticity2;

//rotationmatrices
mat3 rot1;
mat3 rot2;

void main()
{
	speed1 *= rot1;
	speed2 *= rot1;

	speed1.x = 2*((speed1.x * mass1 + speed2.x * mass2)/(mass1+mass2)) - speed1.x;
	speed2.x = 2*((speed1.x * mass1 + speed2.x * mass2)/(mass1+mass2)) - speed2.x;

	speed1 *= elasticity1;
	speed2 *= elasticity2;

	speed1 *= rot2;
	speed2 *= rot2;
}