Before using OpenCVSharp for Unity copy "StreamingAssets" folders to project root (Assets folder) 
so that the project structure is like this:

Assets
|- StreamingAssets
|- OpenCVSharpForUnity
|--- Plugins
|--- Scripts
|--- Examples
|--- ...
|- ...

This is needed because Asset Store requires all asset files to be in a sigle sub-folder, 
but StreamingAssets directory has to be in project root for it to work properly.