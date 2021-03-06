﻿#region License
/*
Copyright (c) 2013-2014 Daniil Rodin of Buhgalteria.Kontur team of SKB Kontur

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;
using SharpRpc.Codecs;
using SharpRpc.Codecs.ReflectionTypes;

namespace SharpRpc.Tests.Codecs
{
    public class MethodInfoCodecTests : MemberInfoCodecTestsBase<MethodInfo>
    {
        protected override IManualCodec<MethodInfo> CreateCodec()
        {
            return new MethodInfoCodec(CodecContainer);
        }

        protected override IEnumerable<MethodInfo> GetMembers(Type type)
        {
            return type.GetMethods().Where(x => !x.ContainsGenericParameters);
        }

        private void DoTest<T>(string methodName)
        {
            foreach (var methodInfo in GetMembers(typeof(T)).Where(x => x.Name == methodName))
                DoTest(methodInfo);
        }

        [Test]
        public void SimpleStatic()
        {
            DoTest<int>("Parse");
            DoTest<Expression>("MakeBinary");
        }

        [Test]
        public void SimpleNonStatic()
        {
            DoTest<string>("GetType");
            DoTest<string>("Clone");
            DoTest<string>("IndexOf");
            DoTest<string>("Split");
        }

        public interface IRefOutMock
        {
            void Do(int a, string b);
            void Do(ref int a, out string b);
        }

        [Test]
        public void RefOut()
        {
            DoTest<IRefOutMock>("Do");
        }

        public interface IGenericMock<T1, T2>
        {
            T1 Do<T3, T4>(int a, T2 b, ref T3 c, out T4 d);
        }

        [Test]
        public void Generic()
        {
            DoTest(typeof(IGenericMock<int, string>).GetMethod("Do").MakeGenericMethod(typeof(int[]), typeof(decimal)));
        }
    }
}