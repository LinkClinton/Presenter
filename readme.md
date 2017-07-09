# Presenter

Present Object to Window.

## Introduction

### GraphicsPipeline `static class`

- Render Object
    ```C#
    GraphicsPipeline.Open(...); // Open GraphicsPipeline

    GraphicsPipeline.PutObject(...); //Put Object to GraphicsPipeline
    
    GraphicsPipeline.Close(); //Close GraphicsPipeline and Present
    //And the GraphicsPipelineState will be clear.
    ```

- Change GraphicsPipelineState
    ```C#
    //If you want to change GraphicsPipeline when GraphicsPipeline is opening.

    GraphicsPipeline.Reset(...);
    ```

- Set Buffer to Shader
    ```C#
    GraphicsPipeline.InputSlot[which] = new ConstantBuffer<T>(...);
    ```

### Surface 

- Create From Hwnd
    ```C#
    Present present = new Present(Hwnd, Windowed);
    ```
- Create Surface
  ```C#
  Surface surface = new Surface(width, height);
  ```
- BackGround 
    ```C#
    Surface surface.BackGround = (1, 1, 1, 1);
    ```

### GraphicsPipelineState
A GraphicsPipelineState.

You must set it before you do anything.

- Create
    ```C#
    GraphicsPipelineState graphicsPipelineState = new GraphicsPipelineState(VertexShader, 
        PixelShader, BufferLayout, RasterizerState, DepthStencilState, BlendState);
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
### InputLayout

- Create From Element
    ```C#
    public struct Element
    {
        public ElementSize Size; // bytes
        public string Tag; //Tag : POSITION,COLOR,NORMAL,TEXCOORD...
    }

    Element[] elements = new Element[2];

    InputLayout layout = new InputLayout(elements);
    ```

### ShaderResource

- Set 
    ```C#
    GraphicsPipeline.InputSlot[which] = new ShaderResource(...);
    ```
- Texture
    ```
    Texture2D texture = Texture2D.FromFile(...);

    Simple Texture Loader.
        Format : R8G8B8A8
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
            new ResourceLayout.Element(ResourceType.ConstantBufferView, 0),
            new ResourceLayout.Element(ResourceType.ShaderResourceView, 0)
        };
    //for this code, we create ResourceLayout for one ConstantBuffer and one ShaderResource.
    ```
- Set Resource by ResourceLayout
    ```C#
    GraphicsPipeline.InputSlot[which] = ...;

    //Set Resource.
    //We support this resource:
    //ConstantBufferView, ShaderResourceView, ConstantBufferViewTable, ShaderResourceViewTable
    ```

### ResourceHeap and ResourceTable

The Heap can create ResourceView from Resource.

We can create ResourceTable from ResourceHeap.

**If you want to set Table,you should set Heap what the Table used.** 

- Create
    ```C#
    ResourceHeap heap = new ResourceHeap(MaxCount in Heap);
    ```

- AddResource
    ```C#
    ResourceHeap.AddResource(...);
    ```
- To ResourceTable
    ```C#
    ResourceTable table = new ResourceTable(heap, start pos in heap);

    //You can think the Table is a pointer,We will set resource from Table's first element.
    ```


## Sample

Look At [**Mico**](https://github.com/LinkClinton/Mico/tree/master/Sample)

Look At [**SampleSet**](https://github.com/LinkClinton/SampleSet)

## Request

- **.NET 4.6.2**
- [**APILibrary**](https://github.com/LinkClinton/APILibrary)
- [**SharpDX**](https://github.com/sharpdx/SharpDX)