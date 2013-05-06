﻿using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;

namespace SharpRpc.Codecs
{
    public static class ILGeneratorExtensions
    {
        public static void Emit_Ldc_I4(this ILGenerator il, int c)
        {
            switch (c)
            {
                case -1: il.Emit(OpCodes.Ldc_I4_M1); return;
                case 0: il.Emit(OpCodes.Ldc_I4_0); return;
                case 1: il.Emit(OpCodes.Ldc_I4_1); return;
                case 2: il.Emit(OpCodes.Ldc_I4_2); return;
                case 3: il.Emit(OpCodes.Ldc_I4_3); return;
                case 4: il.Emit(OpCodes.Ldc_I4_4); return;
                case 5: il.Emit(OpCodes.Ldc_I4_5); return;
                case 6: il.Emit(OpCodes.Ldc_I4_6); return;
                case 7: il.Emit(OpCodes.Ldc_I4_7); return;
                case 8: il.Emit(OpCodes.Ldc_I4_8); return;
            }
            
            if (sbyte.MinValue <= c && c <= sbyte.MaxValue)
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)c);
                return;
            }
            
            il.Emit(OpCodes.Ldc_I4, c);
        }

        public static void Emit_Ldarg(this ILGenerator il, int index)
        {
            switch (index)
            {
                case 0: il.Emit(OpCodes.Ldarg_0); return;
                case 1: il.Emit(OpCodes.Ldarg_1); return;
                case 2: il.Emit(OpCodes.Ldarg_2); return;
                case 3: il.Emit(OpCodes.Ldarg_3); return;
            }

            if (sbyte.MinValue <= index && index <= sbyte.MaxValue)
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)index);
                return;
            }

            il.Emit(OpCodes.Ldarg, index);
        }

        public static void Emit_IncreasePointer(this ILGenerator il, LocalBuilder dataPointerVar, int distance)
        {
            il.Emit(OpCodes.Ldloc, dataPointerVar);
            il.Emit_Ldc_I4(distance);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Stloc, dataPointerVar);
        }

        public static void Emit_IncreasePointerDynamic(this ILGenerator il, LocalBuilder dataPointerVar, LocalBuilder distanceVar)
        {
            il.Emit(OpCodes.Ldloc, dataPointerVar);
            il.Emit(OpCodes.Ldloc, distanceVar);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Stloc, dataPointerVar);
        }

        public static void Emit_DecreaseInteger(this ILGenerator il, LocalBuilder remainingBytesVal, int distance)
        {
            il.Emit(OpCodes.Ldloc, remainingBytesVal);
            il.Emit_Ldc_I4(distance);
            il.Emit(OpCodes.Sub);
            il.Emit(OpCodes.Stloc, remainingBytesVal);
        }

        public static void Emit_DecreaseIntegerDynamic(this ILGenerator il, LocalBuilder remainingBytesVal, LocalBuilder distanceVar)
        {
            il.Emit(OpCodes.Ldloc, remainingBytesVal);
            il.Emit(OpCodes.Ldloc, distanceVar);
            il.Emit(OpCodes.Sub);
            il.Emit(OpCodes.Stloc, remainingBytesVal);
        }

        private static readonly ConstructorInfo ExceptionConstructor = typeof(InvalidDataException).GetConstructor(new[] { typeof(string) });
        public static void Emit_ThrowUnexpectedEndException(this ILGenerator il)
        {
            il.Emit(OpCodes.Ldstr, "Unexpected end of request data");// throw new InvalidDataException(
            il.Emit(OpCodes.Newobj, ExceptionConstructor);           //     "Unexpected end of request data")
            il.Emit(OpCodes.Throw);
        }

        private static void Emit_LoadSize(this ILGenerator il, IEmittingCodec codec, Action<ILGenerator> load)
        {
            if (codec.HasFixedSize)
            {
                il.Emit_Ldc_I4(codec.FixedSize);
            }
            else
            {
                load(il);
                codec.EmitCalculateSize(il);
            }
        }

        public static void Emit_LoadSize(this ILGenerator il, IEmittingCodec codec)
        {
            if (codec.HasFixedSize)
            {
                il.Emit(OpCodes.Pop);
                il.Emit_Ldc_I4(codec.FixedSize);
            }
            else
            {
                codec.EmitCalculateSize(il);
            }
        }

        public static void Emit_LoadSize(this ILGenerator il, IEmittingCodec codec, int argIndex)
        {
            Emit_LoadSize(il, codec, lil => lil.Emit_Ldarg(argIndex));
        }

        public static void Emit_LoadSize(this ILGenerator il, IEmittingCodec codec, LocalBuilder localVar)
        {
            Emit_LoadSize(il, codec, lil => lil.Emit(OpCodes.Ldloc, localVar));
        }

        private static LocalBuilder Emit_PinArray(this ILGenerator il, LocalVariableCollection locals, Action<ILGenerator> load)
        {
            var argsDataPointerVar = locals.GetOrAdd("pinnedDataPointer",
                lil => lil.DeclareLocal(typeof(byte*), true));
            load(il);
            il.Emit_Ldc_I4(0);
            il.Emit(OpCodes.Ldelema, typeof(byte));
            il.Emit(OpCodes.Stloc, argsDataPointerVar);
            return argsDataPointerVar;
        }

        public static LocalBuilder Emit_PinArray(this ILGenerator il, LocalVariableCollection locals)
        {
            return Emit_PinArray(il, locals, lil => { });
        }

        public static LocalBuilder Emit_PinArray(this ILGenerator il, LocalVariableCollection locals, LocalBuilder localVar)
        {
            return Emit_PinArray(il, locals, lil => lil.Emit(OpCodes.Ldloc, localVar));
        }

        public static LocalBuilder Emit_PinArray(this ILGenerator il, LocalVariableCollection locals, int argIndex)
        {
            return Emit_PinArray(il, locals, lil => lil.Emit_Ldarg(argIndex));
        }

        public static void Emit_UnpinArray(this ILGenerator il, LocalBuilder pinnedPointerVar)
        {
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Stloc, pinnedPointerVar);
        }
    }
}