
**Real Time Sphere Collisions in Videogames**

This section will dive into the mathematical underpinnings of sphere collisions, laying out my understanding of their dynamics in a 3D space. Key topics will include the representation of spheres, collision detection, and collision resolution.

**Sphere Collision Basics**

In 3D gaming environments, spheres are often used to represent objects due to their mathematical simplicity and ease of calculation. Collision with complex shapes, often referred to as rigid-body collisions, are much more complicated and therefore require more computing power. Spheres are used in scenarios where speed trumps accuracy, often sphere collision boxes will be used to loosely encapsulate a game object even if that object doesn’t directly resemble a sphere. An example would be using a sphere collision box for a character in a fast paced shoot-em-up, where the physics of the character interacting with the world is secondary to simply detecting if the character has been hit.  
The basic properties of a sphere in these simulations include its position in 3D space, defined by coordinates (x, y, z) at the centre of the sphere, and its physical attributes such as velocity, mass, and radius. These factors are crucial in determining how the sphere interacts with other objects. For instance, the mass and velocity of a sphere directly influence the subsequent movement of both objects involved in a collision.

**Types of Sphere Collisions**

**Sphere to Stationary Sphere**

Collisions between a moving sphere and a stationary one are common scenarios in video games, from simple ball games to complex physics puzzles. The key to simulating this interaction lies in accurately detecting when and where the spheres will collide. This involves calculating the trajectories and anticipating the point of impact.

![image](https://github.com/user-attachments/assets/98b2103c-1ef4-4927-a7dc-01b7182eff23)

The first step in detecting a collision is determining if the trajectory of the moving sphere is on a collision course with the stationary sphere. The diagram above will act as a visualisation during my explanation. The stationary sphere is represented by s2, the moving sphere by s1. During the code we will be checking spheres against one another so we will know the position of s1 and s2, therefore we will know B.  
We will also know the direction of A as this represents the velocity of s1.  
We need to find the angle q so that we can find C. Finding C is the key to determining if a collision will occur as we can then check if C is less than the sum of the radii of the two spheres. If this is the case then we know that on s1’s current course it will eventually end up inside s2, or in other words a collision will occur and we should proceed with further checks.  
<br/>To find q we can use vector based trigonometry which will provide the correct value no matter the orientation of s1 in relation to s2.

cos⁡(q)=(B⋅A)/|B||A|
![image](https://github.com/user-attachments/assets/e4cf3b0e-8713-465f-8e59-469415a82ac5)

<br/>We need the actual angle of q in radians, the above equation will give us a value ranging from -1 to 1. Instead we will use the inverse cosine.  

q=arccos⁡((B⋅A)/(∣B∣∣A∣))
![image](https://github.com/user-attachments/assets/f8738731-50fa-427c-81c4-12093aa8e167)

<br/>Now we can use basic trigonometry principles to find the equation needed to determine C.

sin⁡(q)=A/C
![image](https://github.com/user-attachments/assets/2ae0fcc8-6b8b-44f7-8c59-7b483458d374)

<br/>To get C from this we need to rearrange the equation.

C=sin⁡(q)A
![image](https://github.com/user-attachments/assets/fdb40b95-4c51-430f-9c2e-b03bf397af47)

<br/>Now in code we can check if C is less than the sum of the radii, if not we will end checks for this frame. However if it is less than we will proceed to determining whether the collision will occur this frame.
![image](https://github.com/user-attachments/assets/22d829ae-aeea-4f95-9eb0-9c020a55b63c)

Now that we know s1 is going to collide we can work on finding the point of collision. We do this by finding Ac, the length of Vector A from s1 to the point of collision. The magnitude of which is then compared to the magnitude of the velocity to determine if the collision occurs within this frame.  
We need to find d, this is somewhat trivial since we know that C is perpendicular to A so the angle between C and d must be 90 degrees. We also know the length of the Hypotenuse by using the logic that at the point the spheres collide their radii can be thought of as touching, therefore the hypotenuse is equal to the sum of the radii.

d=√((r_1+r_2 )^2-C^2)
![image](https://github.com/user-attachments/assets/aa6098b9-ea71-4564-84b9-23b934c35a15)

<br/>To find the length of Ac we can use the following equation.

|Ac|=cos⁡〖(q)|B|-d
![image](https://github.com/user-attachments/assets/165efc18-8c83-447b-81df-58fad00edf57)

<br/>If the magnitude of Ac is less than the magnitude of the s1’s velocity (A) then we know that a collision will occur this frame and we can move onto the next step. Furthermore since we know that A is the full length that the sphere would move this frame, we can use the magnitude of Ac to find out at what point the collision occurs in subframe time which is used in the collision resolution step.

**Sphere to Moving Sphere**

When two spheres in motion collide, the dynamics become more complex. The collision outcome depends on various factors, including the speed, direction, and mass of both spheres. This scenario requires an understanding of momentum conservation and kinetic energy transfer, principles that are fundamental to accurately simulating realistic collisions. Such simulations can range from simple elastic collisions, where energy is conserved, to more complex inelastic ones, where energy is dissipated in forms like sound or deformation.

The initial step involves calculating the relative velocity and position vectors between two spheres. These vectors are pivotal in understanding the dynamic interaction of the spheres.

V ⃗rel=V ⃗s1-V ⃗s2<br/>P ⃗rel=P ⃗s1-P ⃗s2
![image](https://github.com/user-attachments/assets/9df1662b-1b0f-4e97-8a1c-bc43ff78aeb7)

The relative velocity is the velocity of one sphere relative to the other. Its importance lies in determining how fast and in what direction one sphere is moving towards or away from the other. A relative velocity of zero would mean that both spheres are moving at the same speed and direction, indicating no potential for a collision.  
The relative position represents the vector distance between the centres of the two spheres. This is crucial for determining how far apart the spheres are and in what direction they are relative to each other.  
These vectors provide the necessary information to calculate the quadratic equation used in collision detection algorithms.  
<br/>The general form of a quadratic equation is expressed as follows. 

ax2+bx+c=0
![image](https://github.com/user-attachments/assets/4f89e273-83ce-4904-8e71-393a59513b15)

<br/>In our case we use the discriminate in the quadratic equation to quickly assess whether a real collision is possible between the spheres.

Δ=b^2-4ac
![image](https://github.com/user-attachments/assets/efcf1f30-e5fb-48e8-acd3-17797d8dc6ab)

<br/>If the discriminate is positive it means the paths of the spheres intersect at 2 points, suggesting a collision will occur. If the discriminate is zero it means the spheres touch each other at one point along their course, also indicating a collision.  
If the discriminate is negative then the courses of the spheres do not cross and we assume that no collision will take place.

<br/>**Coefficient a (Rate of Distance Change):** This term, derived from the dot product of relative velocity with itself, indicates the rate at which the distance between two spheres is changing. It's pivotal in determining if the spheres are on a collision course.

a=V ⃗rel⋅V ⃗rel
![image](https://github.com/user-attachments/assets/02066671-e403-47f6-8712-1e14a80f25b3)

<br/>**Coefficient b (Rate of Approach):** This coefficient is calculated as twice the dot product of relative velocity and relative position. It links the rate at which spheres are moving towards or away from each other with their current separation.

b=2(V ⃗rel⋅P ⃗rel)
![image](https://github.com/user-attachments/assets/a30e72c3-d3cf-4cf4-9504-665c94fa7ff5)

<br/>**Coefficient c (Initial Separation and Radii):** Represents the squared initial distance between the spheres reduced by the squared sum of their radii. It's crucial for establishing the initial proximity and potential for collision.

c=P ⃗rel⋅P ⃗rel-(r_1+r_2 )^2
![image](https://github.com/user-attachments/assets/9a813df2-cd2a-4f3d-a101-1083747f33b3)

<br/>If the discriminate is non-negative we know that a collision is going to occur along the paths of the spheres but we do not yet know if that collision will occur this frame. To calculate this we can make further use of the quadratic formula and calculate for t.

t_(1,2)=(-b ± √(b^2-4ac))/2a
![image](https://github.com/user-attachments/assets/d11b206f-4e5f-47f7-8081-c7f45d7b5366)

<br/>This indicates two solutions which gives both t1 and t2 as separate values. We then have to check if either falls within the current update interval. To do this we get the smallest of the two solutions and check it against the delta time. The smallest of these solutions is the exact moment that the collision occurs and can be used in subframe calculations during the collision resolution.

To calculate the collision response for 2 spheres colliding we first need to get the collision normal, this tells us in which dimensions the collision is occurring relative to each other.  

The post-collision velocities of the two spheres is finally calculated using the mathematical equation for an elastic collision between two spheres in three dimensions.  

Where _n_ is equal to collision normal.  
The dot product calculates the relative velocity along the collision normal. The result is then scaled by the factor for the first sphere and for the second sphere, which accounts for the mass ratio between the spheres. This scaled value is then multiplied by the collision normal to get the change in velocity along the line of impact, which is subtracted from and added to to get the final velocities.  
<br/>Finally we multiply the resulting velocities by an artificial coefficient of restitution to achieve the final velocities. The coefficient of restitution is a value that describes how much kinetic energy remains after a collision. This helps us take energy out of the system to simulate energy lost to heat or friction during the collision event, a value of 1 would mean no energy lost, a value higher than 1 would simulate energy gained during the collision, and a value below 1 is used to simulate energy lost.  
a\\

**Sphere to Plane**

This section provides an analytical examination of the algorithms used to detect and resolve collisions between spheres and planes in 3D gaming environments, emphasizing the mathematical foundations.  
In the context of video game physics, understanding the collision dynamics between a sphere and a plane requires a blend of vector mathematics and principles of kinematics. The collision outcome depends on various factors, including the velocity of the sphere, its orientation relative to the plane, and the properties of both the sphere and the plane.

The first step is to establish the relative position and velocity of the sphere with respect to the plane. This vector represents the distance and direction of the sphere from a point on the plane.  
Then we must check if the fot product of the spheres velocity and the planes normal vector is positive, this checks if the sphere is moving towards the plane.  

Like before we are at a point where a collision is possible but we need to make further checks to determine if a collision will occur this frame. Firstly we will perform a distance check.  
Finally we can calculate the time of collision so that we can check it against the simulation time step.  
This formula determines when the sphere’s surface first contacts the plane, if the collision time is within the fames duration we can proceed to the collision resolution step.  
Just before we go to the collision step we move the sphere along its velocity by collision time, this places the sphere on the surface of the plane but it will not be visualised since the rendering will only display the spheres final position after the collision.  
<br/>To resolve the collision the first step is to normalise the planes normal vector. This ensures a consistent reference for calculating the spheres response upon collision.  
Since we are dealing with a simple plane we can simply use the reflection angle, giving us a much easier calculation than in the previous collision resolutions.  
However in this perfect reflection the size of the velocity vector remains the same, akin to conserving all momentum after the collision. We use the artificial coefficient of restitution to trim the magnitude of the velocity, thereby simulating a loss in energy.  
Finally we move the sphere along its new velocity for the remainder of the frame time, which ensures continuous momentum and realistic mathematical modelling of the collision which could otherwise be lost.

