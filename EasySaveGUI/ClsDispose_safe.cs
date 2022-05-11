using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace EasySave {
    public class ClsDispose_safe
    {
        private bool bDisposed = false;
        SafeHandle objSafeHandle = new SafeFileHandle(IntPtr.Zero, true);

        public void Dispose1()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool bDispose)
        {
            if (bDisposed)
                return;

            if (bDispose)
            {
                objSafeHandle.Dispose();
            }

            bDisposed = true;
        }
    }
}
