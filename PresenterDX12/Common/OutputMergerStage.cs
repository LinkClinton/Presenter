using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class OutputMergerStage
    {
        public void Reset()
        {
            
        }

        public DepthStencilState DepthStencilState => GraphicsPipeline.State.DepthStencilState;

        public BlendState BlendState => GraphicsPipeline.State.BlendState;

        public (float red,float green,float blue,float alpha) BlendFactor
        {
            set => GraphicsPipeline.ID3D12GraphicsCommandList.BlendFactor = new SharpDX.Mathematics.Interop.RawVector4(
                value.red, value.green, value.blue, value.alpha);
        }
    }

    class StaticOutputMergerStage: OutputMergerStage { }

    public static partial class GraphicsPipeline
    {
        private static OutputMergerStage outputMergerStage = new StaticOutputMergerStage();

        public static OutputMergerStage OutputMergerStage => outputMergerStage;
    }
}
