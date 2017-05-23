# Presenter

Present Object to Window.

## Introduction

### Manager `static class`

- Render Object
    ```C#
    Manager.ClearObject(); //clear Surface and Set White Color.

    Manager.DrawObjectIndexed(...); //DrawBuffer.
    Manager.PutObject(...); //Draw Object such as line,rectangle...
    
    Manager.FlushObject(); //Flush Object and Present it.
    ```
- Set Target Surface
    ```C#
    Manager.Surface = new Surface(...); //Create Surface to Render.
    ```
- Set Shader
    ```C#
    Manager.VertexShader = new VertexShader(...); //VertexShader

    Manager.PixelShader = new PixelShader(...); //PixelShader
    ```
- Set Buffer to Shader
    ```C#
    Manager.ConstantBuffer[(Shader,WhichID)] = new ConstantBuffer<T>(...);

    //Shader is a VertexShader or PixelShader...
    //For example: Manager.ConstantBuffer[(Manager.VertexShader,0)]
    ```

### Surface 

- Create From Hwnd
    ```C#
    Surface surface = new Surface(Hwnd, Windowed);
    ```

### Buffer

- ConstantBuffer
    ```C#
    Buffer buffer = new ConstantBuffer<T>(ref data); //data must be struct
    Buffer buffer = new ConstantBuffer<T>(data[]); //data must be array of struct

    ConstantBuffer<T> buffer = new ConstantBuffer<T>(ref data);
    ConstantBuffer<T> buffer = new ConstantBuffer<T>(data[]);

    //Other Way to Set
    VertexShader.ConstantBuffer[which] = new ConstantBuffer<T>(...);
    PixelShader.ConstantBuffer[which] = new ConstantBuffer<T>(...);
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
        public string Tag; //Tag : POSITION,COLOR,NORMAL,TEXCOOD...
    }

    Element[] elements = new Element[2];

    BufferLayout layout = new BufferLayout(elements);
    ```

- Set 
    ```C#
    Manager.BufferLayout = new BufferLayout(...);
    ```

## Request

- **.NET 4.7**
- [**APILibrary**](https://github.com/LinkClinton/APILibrary)
- [**SharpDX**](https://github.com/sharpdx/SharpDX)