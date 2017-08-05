using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public static partial class GraphicsPipeline
    {
        private static SharpDX.Direct3D12.GraphicsCommandList graphicsCommandList;

        private static bool isOpened;
        
        static GraphicsPipeline()
        {
            ID3D12GraphicsCommandList = Engine.ID3D12Device.CreateCommandList(SharpDX.Direct3D12.CommandListType.Direct,
                Engine.ID3D12CommandAllocator, null);

            ID3D12GraphicsCommandList.Close();
        }

        public static void Open(GraphicsPipelineState GraphicsPipelineState, Present Target)
        {
#if DEBUG
            if (IsOpened is true) throw new NotImplementedException("GraphicsPipeline has opened");
#endif
            present = Target;

            graphicsPipelineState = GraphicsPipelineState;

            Engine.ID3D12CommandAllocator.Reset();

            ID3D12GraphicsCommandList.Reset(Engine.ID3D12CommandAllocator,
                graphicsPipelineState.ID3D12GraphicsPipelineState);

            ID3D12GraphicsCommandList.SetGraphicsRootSignature(graphicsPipelineState.ResourceLayout.ID3D12RootSignature);

            present.ResetViewport();

            present.ResetResourceView();
        }

        public static void Open(GraphicsPipelineState GraphicsPipelineState, TextureFace Target)
        {
#if DEBUG
            if (IsOpened is true) throw new NotImplementedException("GraphicsPipeline has opened");
#endif
            textureFace = Target;

            graphicsPipelineState = GraphicsPipelineState;

            Engine.ID3D12CommandAllocator.Reset();

            ID3D12GraphicsCommandList.Reset(Engine.ID3D12CommandAllocator,
                graphicsPipelineState.ID3D12GraphicsPipelineState);

            ID3D12GraphicsCommandList.SetGraphicsRootSignature(graphicsPipelineState.ResourceLayout.ID3D12RootSignature);

            textureFace.ResetViewport();

            textureFace.ResetResourceView();
        }

        public static void Close()
        {
            present?.ClearState();
            textureFace?.ClearState();

            ID3D12GraphicsCommandList.Close();

            Engine.ID3D12CommandQueue.ExecuteCommandList(ID3D12GraphicsCommandList);

            present?.Presented();
            textureFace?.Presented();

            InputAssemblerStage.Reset();
            VertexShaderStage.Reset();
            PixelShaderStage.Reset();
            OutputMergerStage.Reset();

            present = null;
            textureFace = null;
            graphicsPipelineState = null;

            isOpened = false;
        }

        public static void WaitFlush()
        {
            Engine.Wait(Engine.ID3D12CommandQueue);
        }

        public static bool IsOpened => isOpened;

        internal static SharpDX.Direct3D12.GraphicsCommandList ID3D12GraphicsCommandList
        {
            private set => graphicsCommandList = value;
            get => graphicsCommandList;
        }
    }
}
