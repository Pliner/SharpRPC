﻿#region License
/*
Copyright (c) 2013 Daniil Rodin of Buhgalteria.Kontur team of SKB Kontur

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
using NUnit.Framework;
using SharpRpc.Codecs;

namespace SharpRpc.Tests.Codecs
{
    [TestFixture]
    public class CodecContainerTests
    {
        private CodecContainer codecContainer;

        [SetUp]
        public void Setup()
        {
            codecContainer = new CodecContainer();
        }

        [Test]
        public void Emitting()
        {
            var codec1 = codecContainer.GetEmittingCodecFor(typeof(decimal));
            var codec2 = codecContainer.GetEmittingCodecFor(typeof(decimal));
            Assert.That(codec1, Is.EqualTo(codec2));
        }

        [Test]
        public void Manual()
        {
            var codec1 = codecContainer.GetManualCodecFor<decimal>();
            var codec2 = codecContainer.GetManualCodecFor<decimal>();
            Assert.That(codec1, Is.EqualTo(codec2));
        }

        [Test]
        public void ManualExceptions()
        {
            var codec1 = codecContainer.GetManualCodecFor<Exception>();
            var codec2 = codecContainer.GetManualCodecFor<Exception>();
            Assert.That(codec1, Is.EqualTo(codec2));
        }
    }
}