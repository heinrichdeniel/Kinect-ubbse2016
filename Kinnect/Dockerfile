# Docker build script for the Kinect.

# Start with microsoft/dotnet image
FROM microsoft/dotnet

# Copy sources to virtual machine
# local folder will become /usr/src/kinect on Docker image

RUN mkdir /usr/src/kinect
COPY KinectControl/bin/Release/ /usr/src/kinect
