# Presenter

Present Object to Window.

## Introduction

### Manager `static class`

- Render Object
    ```C#
    Manager.ClearObject(); //clear Surface and Set White Color.

    Manager.DrawObjectIndexed(...); //DrawBuffer.
    
    Manager.FlushObject(); //Flush Object and Present it.
    ```
- Set Target Surface
    ```C#
    Manager.Surface = new Surface(...); //Create Surface to Render.
    ```
- Set GraphicsPipelineState
    ```C#
    Manager.GraphicsPipelineState = new GraphicsPipelineState(...);
    ```

- Set Buffer to Shader
    ```C#
    ResourceLayout.InputSlot[which] = new ConstantBuffer<T>(...);
    ```

### Surface 

- Create From Hwnd
    ```C#
    Surface surface = new Surface(Hwnd, Windowed);
    ```
- BackGround 
    ```C#
    Surface surface.BackGround = (1, 1, 1, 1);
    ```
- Reset  
    On Windows size changed, you can reset the surface.
    ```C#
    Surface surface.Reset(new_width, new_height, windowed = true);
    ```

### GraphicsPipelineState
A GraphicsPipelineState.

You must set it before you do anything.

- Create
    ```C#
    GraphicsPipelineState graphicsPipelineState = new GraphicsPipelineState(VertexShader, 
        PixelShader, BufferLayout);
    ```

### Buffer

- ConstantBuffer
    ```C#
    Buffer buffer = new ConstantBuffer<T>(ref data); //data must be struct
    Buffer buffer = new ConstantBuffer<T>(data[]); //data must be array of struct

    ConstantBuffer<T> buffer = new ConstantBuffer<T>(ref data);
    ConstantBuffer<T> buffer = new ConstantBuffer<T>(data[]);
    ```
- VertexBuffer
    ```C#
    Buffer buffer = new VertexBuffer<T>(data[]); //data must be array of struct

    VertexBuffer<T> buffer = new VertexBuffer<T>(data[]);

    //Set
    Manager.VertexBuffer = new VertexBuffer<T>(data[]);
    ```
- IndexBuffer
    ```C#
    Buffer buffer = new IndexBuffer<T>(data[]); //T must be 4 bytes such as uint,int...

    IndexBuffer<T> buffer = new IndexBuffer<T>(data[]);

    //Set 
    Manager.IndexBuffer = new IndexBuffer<T>(data[]);
    ```
### BufferLayout

- Create From Element
    ```C#
    public struct Element
    {
        public ElementSize Size; // bytes
        public string Tag; //Tag : POSITION,COLOR,NORMAL,TEXCOORD...
    }

    Element[] elements = new Element[2];

    BufferLayout layout = new BufferLayout(elements);
    ```

### ShaderResource

- Set 
    ```C#
    ResourceLayout.InputSlot[which] = new ShaderResource(...);
    ```
- Texture
    ```
    Texture texture = new Texture(filename);

    Simple Texture Loader.
        Format : DXGI_R8G8B8A8_UNORM
        CpuAccessFlags: None
        MipLevels : 1
        Support: WIC Supported
    ```
### ResourceLayout

- Create
    ```C#
    ResourceLayout.Element[] resouceElements
        = new ResourceLayout.Element[]
        {
            new ResourceLayout.Element(ResourceLayout.ResourceType.ConstantBufferView, 0),
            new ResourceLayout.Element(ResourceLayout.ResourceType.ShaderResourceView, 0)
        };
    //for this code, we create ResourceLayout for one ConstantBuffer and one ShaderResource.
    ```
- Set Resource by ResourceLayout
    ```C#
    Manager.ResourceInput[InputSlot] = ...//which kind resource you create
    
    //for example.
    Manager.ResourceInput[0] = Buffer;
    Manager.ResourceInput[1] = Texture;

    //we Set a ConstantBuffer in 0 InputSlot
    //we Set a ShaderResource in 1 InputSlot
    ```


## Sample

Look At [**Mico**](https://github.com/LinkClinton/Mico/tree/master/Sample)

## Request

- **.NET 4.7**
- [**APILibrary**](https://github.com/LinkClinton/APILibrary)
- [**SharpDX**](https://github.com/sharpdx/SharpDX)