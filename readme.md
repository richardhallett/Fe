Fe
==

Overview
--------

Fe is a generic cross platform API agnostic renderering library.
It's a personal research project, using managed code for high performance rendering.

**Please note there is no stable release because it's a research project.**

The concept was partly inspired by the excellent C++ renderer [BGFX](https://github.com/bkaradzic/bgfx) as well as articles on parallel rendering by the bitsquid team (before autodesk) and other articles on the topic.

## Things:

### Multi-threaded command submission
Commands are instruction sets with data that can be created on any thread.
They are not bound to one specific thread, because they are submitted and sorted before being processed sequentially (or possibly not with more advanced rendereres in the future) it means you can do a lot of setup in your code in parallel safely.

### Buckets
Use different command buckets for different aspects of the renderer, possibly for sorting or creation in specific different threads.

### Submission sorting
A custom sort key can be passed when creating commands that will be used to sort the commands in a bucket, this is a RADIX sort behind the scenes.

### Cross Platform
It's aimed to be cross platform, it fully supports .NET Core and actually runs pretty well.
You can pass through a window handle created natively on your environment or a window manager like SDL, everything else cross platform is then currently handled by OpenTK.

*Currently only supports OpenGL 3.3+ via OpenTK*

## Building

Visual studio or dotnet build from CLI will work with the provided solution/project files.

## Examples

* Basics - Combined simple examples for showing some of the simpler features of the renderer
* Dynamic - Shows dynamic mesh updating
* SplitScreen - How to do splitscreen rendering
* StressTest - Based on the BGFX samples a way of comparing performance.