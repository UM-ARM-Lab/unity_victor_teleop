When building for Android it is recommended to use IL2CPP Scripting Backend.
This can be adjusted from Edit => Project Settings => Player => Android Settings => Other Settings (configuration section)

You will need Android NDK for this. When starting the build Unity will ask you for the path to the required NDK folder.
Once you get the prompt click "Download" and NDK will be downloaded automatically. When asked to select the NDK
take care to not select the parent NDK folder, but the child folder inside. Example:

CORRECT => D:\Downloads\android-ndk-r13b-windows-x86_64\android-ndk-r13b
INCORRECT => D:\Downloads\android-ndk-r13b-windows-x86_64