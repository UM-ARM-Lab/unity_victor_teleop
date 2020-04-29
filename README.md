# Unity Teleop (for ARM lab's "Victor" robot)
This is the unity portion use for teleoperating a robot.
Want to learn how to use this package to teleop your robot? Check out the [tutorials!](https://github.com/UM-ARM-Lab/unity_victor_teleop/wiki/Tutorial-1:-Set-up-Unity-package)

## Brief description

This is a Unity project for controlling the armlab's Victor robot using an HTC vive.
This unity project connects to [VR Teleop](https://github.com/UM-ARM-Lab/vr_teleop), connecting the Unity interface to the ROS ecosystem. 
Video from a Kinect enter as ROS rgb and depth images.
The current robot position enters as ROS robot_state messages
The commanded gripper poses exit as `geometry_msgs::pose`.

## Installation

1. Install unity (Free version)
2. Clone this repo
3. Open this repo as a unity project
4. Import `SteamVR` from the unity asset store
   - `Window -> Asset Store` and search for SteamVR
   
(If you are having issues, check out the [tutorials](https://github.com/UM-ARM-Lab/unity_victor_teleop/wiki/Tutorial-1:-Set-up-Unity-package))

### Ubuntu Setup
To communicate to ROS:
Set up an ubuntu computer with ROS, optionally a VM.
Follow the instruction provided by ROSSharp [https://github.com/siemens/ros-sharp/wiki/User_Inst_ROSOnUbuntu]

Set up VR teleop on ubuntu: https://github.com/UM-ARM-Lab/vr_teleop

For more information, see the [wiki](https://github.com/UM-ARM-Lab/unity_victor_teleop/wiki)
