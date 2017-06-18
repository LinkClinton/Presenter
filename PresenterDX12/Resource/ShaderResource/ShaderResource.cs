using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class ShaderResource : Resource
    {
        protected SharpDX.Direct3D12.ShaderResourceViewDescription resourceview =
            new SharpDX.Direct3D12.ShaderResourceViewDescription();

        protected ResourceFormat pixelFormat;

        private const int ComponentMappingMask = 0x7;
        private const int ComponentMappingShift = 3;
        private const int ComponentMappingAlwaysSetBitAvoidingZeromemMistakes = (1 << (ComponentMappingShift * 4));

        protected static int ComponentMapping(int src0, int src1, int src2, int src3)
        {
            return ((((src0) & ComponentMappingMask) |
            (((src1) & ComponentMappingMask) << ComponentMappingShift) |
            (((src2) & ComponentMappingMask) << (ComponentMappingShift * 2)) |
            (((src3) & ComponentMappingMask) << (ComponentMappingShift * 3)) |
            ComponentMappingAlwaysSetBitAvoidingZeromemMistakes));
        }

        protected static int DefaultComponentMapping()
        {
            return ComponentMapping(0, 1, 2, 3);
        }

        protected static int ComponentMapping(int ComponentToExtract, int Mapping)
        {
            return ((Mapping >> (ComponentMappingShift * ComponentToExtract) & ComponentMappingMask));
        }

        internal SharpDX.Direct3D12.ShaderResourceViewDescription ShaderResourceView => resourceview;

        public ResourceFormat PixelFormat => pixelFormat;
    }
}
