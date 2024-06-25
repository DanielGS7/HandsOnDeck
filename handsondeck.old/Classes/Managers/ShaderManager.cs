using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public class ShaderManager
{
    private Dictionary<string, Effect> shaders = new Dictionary<string, Effect>();
    private static ShaderManager _shaderManager = new ShaderManager();

    public static ShaderManager GetInstance
    {
        get { return _shaderManager; }
    }

    private ShaderManager() { }

public void LoadContent(ContentManager content)
{
    try
    {
        shaders["VertexShaderInput"] = content.Load<Effect>("VertexShaderInput");
    }
    catch (ContentLoadException ex)
    {
        Console.WriteLine($"Error loading shader: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An unexpected error occurred: {ex.Message}");
    }
}

    public Effect GetShader(string name)
    {
        if (shaders.ContainsKey(name))
        {
            return shaders[name];
        }
        else
        {
            throw new KeyNotFoundException($"Shader with name {name} not found.");
        }
    }
        public void UpdateSunPosition(GameTime gameTime)
    {
        float time = (float)gameTime.TotalGameTime.TotalHours / 24.0f;
        float x = MathF.Cos(time * MathF.PI * 2);
        float z = MathF.Sin(time * MathF.PI * 2);
        
        shaders["VertexShaderInput"].Parameters["SunPos"].SetValue(new Vector3(x, 0.5f, z));
    }
}