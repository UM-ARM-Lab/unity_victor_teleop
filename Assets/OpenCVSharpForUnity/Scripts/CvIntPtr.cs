using System;
using System.Runtime.InteropServices;
using UnityEngine;


/// <summary>
/// Helper class that enables IDisposable usage with System.IntPtr.
/// </summary>
public class CvIntPtr : IDisposable
{
    private IntPtr ptr;
    private GCHandle handle;


    public CvIntPtr() { }


    public CvIntPtr(IntPtr ptr)
    {
        this.ptr = ptr;
    }


    public CvIntPtr(ref byte[] byteArray)
    {
        ptr = GetIntPtr(byteArray);
    }


    public IntPtr Ptr
    {
        get
        {
            return handle.AddrOfPinnedObject();
        }
        set
        {
            ptr = value;
        }
    }
        

    /// <summary>
    /// Get object's System.IntPtr.
    /// </summary>
    public System.IntPtr GetIntPtr<T>(T obj)
    {
        handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
        return handle.AddrOfPinnedObject();
    }


    public void Dispose()
    {
        Cleanup();
        GC.SuppressFinalize(this);
    }


    private void Cleanup()
    {
        if (handle != null && handle != default(GCHandle))
            handle.Free();

        Marshal.FreeHGlobal(ptr);
    }


    ~CvIntPtr()
    {
        Cleanup();
    }
}