﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public static class Engine
    {
        private static SharpDX.Direct2D1.Factory1 d2d1Factory;

        private static SharpDX.WIC.ImagingFactory imagingFactory;

        private static SharpDX.Direct3D11.Device device;
        private static SharpDX.Direct3D11.DeviceContext context;
        private static SharpDX.Direct3D11.DeviceContext immediateContext;

        static Engine()
        {
#if DEBUG
            ID3D11Device = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware,
                 SharpDX.Direct3D11.DeviceCreationFlags.Debug);
#else
            ID3D11Device = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware);
#endif
            ID3D11DeviceContext = new SharpDX.Direct3D11.DeviceContext(ID3D11Device);

            immediateContext = ID3D11Device.ImmediateContext;

            ID2D1Factory = new SharpDX.Direct2D1.Factory1(SharpDX.Direct2D1.FactoryType.SingleThreaded);

            ImagingFactory = new SharpDX.WIC.ImagingFactory();
        }

        internal static SharpDX.Direct2D1.Factory1 ID2D1Factory
        {
            private set => d2d1Factory = value;
            get => d2d1Factory;
        }

        internal static SharpDX.WIC.ImagingFactory ImagingFactory
        {
            private set => imagingFactory = value;
            get => imagingFactory;
        }

        internal static SharpDX.Direct3D11.Device ID3D11Device
        {
            private set => device = value;
            get => device;
        }

        internal static SharpDX.Direct3D11.DeviceContext ID3D11DeviceContext
        {
            private set => context = value;
            get => context;
        }

        internal static SharpDX.Direct3D11.DeviceContext ImmediateContext
            => immediateContext;


        public static float DpiX => d2d1Factory.DesktopDpi.Width;
        public static float DpiY => d2d1Factory.DesktopDpi.Height;

        public static float AppScale => (DpiX + DpiY) / 192;
    }
}
