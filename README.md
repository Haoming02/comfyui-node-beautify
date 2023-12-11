<h1 align="center">Deprecated</h1>
<p align="center">Rewriting the features as a native UI button instead~</p>

## ComfyUI Node Beautify
A simple program to format the workflow

### Feature
Round the `size` and the `position` of each node and group in a workflow by a preset value.

### How to Use
1. Download the executable from **Release**
2. Drag and drop the saved workflow `.json` file(s) onto the `.exe`; or launch the `.exe` and enter a path

### Configs
You can clone the repo and edit the source code `Configs.cs` file. 
Within the script, you can modify the `GRID_SIZE` constant to change the values to round to;
You can also add entries to the `FIXED_SIZE` dictionary, to set a constant size for nodes of specified type.
