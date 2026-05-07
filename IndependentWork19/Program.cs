using System;

public interface IRenderer
{
    void Render();
}

public class OpenGLRenderer : IRenderer
{
    public void Render() => Console.WriteLine("Rendering via OpenGL");
}

public class DirectXRenderer : IRenderer
{
    public void Render() => Console.WriteLine("Rendering via DirectX");
}

public class VulkanRenderer : IRenderer
{
    public void Render() => Console.WriteLine("Rendering via Vulkan");
}

public abstract class RendererFactory
{
    protected abstract IRenderer CreateRenderer();

    public void RenderGraphic()
    {
        var renderer = CreateRenderer();
        renderer.Render();
    }
}

public class OpenGLFactory : RendererFactory
{
    protected override IRenderer CreateRenderer() => new OpenGLRenderer();
}

public class DirectXFactory : RendererFactory
{
    protected override IRenderer CreateRenderer() => new DirectXRenderer();
}

public class VulkanFactory : RendererFactory
{
    protected override IRenderer CreateRenderer() => new VulkanRenderer();
}

public class GraphicsEngine
{
    private static GraphicsEngine _instance;
    private RendererFactory _currentFactory;

    private GraphicsEngine() { }

    public static GraphicsEngine Instance => _instance ??= new GraphicsEngine();

    public void SetFactory(RendererFactory factory)
    {
        _currentFactory = factory;
    }

    public void Render()
    {
        if (_currentFactory != null)
        {
            _currentFactory.RenderGraphic();
        }
    }
}

class Program19
{
    static void Main()
    {
        var engine = GraphicsEngine.Instance;

        engine.SetFactory(new OpenGLFactory());
        engine.Render();

        engine.SetFactory(new DirectXFactory());
        engine.Render();

        engine.SetFactory(new VulkanFactory());
        engine.Render();
    }
}