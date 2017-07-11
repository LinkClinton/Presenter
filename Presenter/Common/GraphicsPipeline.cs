using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public static partial class GraphicsPipeline
    {
        private static bool isOpened = false;

        static GraphicsPipeline()
        { 
        }

        public static void Open(GraphicsPipelineState GraphicsPipelineState, Surface target)
        {
            isOpened = true;

            surface = target;

            Reset(GraphicsPipelineState);

            surface.ResetViewPort();

            surface.ResetResourceView();
        }

        public static void Close()
        {
            surface.ClearState();

            using (var commandList = Engine.ID3D11DeviceContext.FinishCommandList(false))
            {
                Engine.ID3D11Device.ImmediateContext.ExecuteCommandList(commandList, false);
            }

            surface.Presented();
            
            InputAssemblerStage.Reset();
            VertexShaderStage.Reset();
            PixelShaderStage.Reset();
            OutputMergerStage.Reset();

            surface = null;
            graphicsPipelineState = null;

            isOpened = false;
        }

        public static bool IsOpened => isOpened;
    }
}
