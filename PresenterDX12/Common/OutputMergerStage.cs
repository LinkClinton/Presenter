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
    }

    class StaticOutputMergerStage: OutputMergerStage { }

    public static partial class GraphicsPipeline
    {
        private static OutputMergerStage outputMergerStage = new StaticOutputMergerStage();

        public static OutputMergerStage OutputMergerStage => outputMergerStage;
    }
}
