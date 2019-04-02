RAMPS!
------


Creating a Ramp
---------------

Right click in your project heirarchy and choose Create->Ramp.


Editing a Ramp
--------------

Select the ramp asset in the project editor, and adjust the settings.
The ramp will update automatically when values are changed.


Using a Ramp
------------

You can use the .Texture property on a ramp asset at runtime if you would
like to generate the ramp textures on the fly and assign them in via code.

If you would rather not use the ramp asset directly, select the ramp then
click the bake or replace buttons which will generate a new ramp or update
an existing ramp texture. You can then use these textures as normal.

The RampTexture component allows you to use a ramp at runtime on objects
with a single material. The material must use a shader which has a "_Ramp"
texture name. If you want to assign the ramp texture to a different name,
use the RampTextureAdvanced component, which allows you to specify an
arbitrary texture name.


Support
-------

Please contact support@differentmethods.com with any questions or problems.