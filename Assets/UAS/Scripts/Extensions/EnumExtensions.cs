using System;
using System.Runtime.CompilerServices;

namespace UAS
{
    public static class EnumExtensions
    {
        public static unsafe T SetFlags<T>(this T value, T flags) where T : unmanaged, Enum
        {
            if (sizeof(T) == 1)
                return UnsafeValue.As<byte, T>((byte)(Unsafe.As<T, byte>(ref value) | Unsafe.As<T, byte>(ref flags)));
            if (sizeof(T) == 2)
                return UnsafeValue.As<short, T>((short)(Unsafe.As<T, short>(ref value) | Unsafe.As<T, short>(ref flags)));
            if (sizeof(T) == 4)
                return UnsafeValue.As<int, T>(Unsafe.As<T, int>(ref value) | Unsafe.As<T, int>(ref flags));
            if (sizeof(T) == 8)
                return UnsafeValue.As<long, T>(Unsafe.As<T, long>(ref value) | Unsafe.As<T, long>(ref flags));
            
            throw new NotSupportedException();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T ClearFlags<T>(this T value, T flags) where T : unmanaged, Enum
        {
            if (sizeof(T) == 1)
                return UnsafeValue.As<byte, T>((byte)(Unsafe.As<T, byte>(ref value) & ~Unsafe.As<T, byte>(ref flags)));
            if (sizeof(T) == 2)
                return UnsafeValue.As<short, T>((short)(Unsafe.As<T, short>(ref value) & ~Unsafe.As<T, short>(ref flags)));
            if (sizeof(T) == 4)
                return UnsafeValue.As<int, T>(Unsafe.As<T, int>(ref value) & ~Unsafe.As<T, int>(ref flags));
            if (sizeof(T) == 8)
                return UnsafeValue.As<long, T>(Unsafe.As<T, long>(ref value) & ~Unsafe.As<T, long>(ref flags));

            throw new NotSupportedException();
        }
        
        private static class UnsafeValue
        {
            public static TTo As<TFrom, TTo>(TFrom source) => Unsafe.As<TFrom, TTo>(ref source);
        }
    }
}