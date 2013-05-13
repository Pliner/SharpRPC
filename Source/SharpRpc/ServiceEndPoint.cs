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

namespace SharpRpc
{
    public struct ServiceEndPoint : IEquatable<ServiceEndPoint>
    {
        public string Protocol;
        public string Host;
        public int Port;

        public ServiceEndPoint(string protocol, string host, int port)
        {
            Protocol = protocol;
            Host = host;
            Port = port;
        }

        public static bool Equals(ServiceEndPoint ep1, ServiceEndPoint ep2)
        {
            return ep1.Protocol == ep2.Protocol && ep1.Host == ep2.Host && ep1.Port == ep2.Port;
        }

        public static bool operator ==(ServiceEndPoint ep1, ServiceEndPoint ep2)
        {
            return string.Equals(ep1, ep2) && ep1.Port == ep2.Port;
        }

        public static bool operator !=(ServiceEndPoint ep1, ServiceEndPoint ep2)
        {
            return !(ep1 == ep2);
        }

        public bool Equals(ServiceEndPoint other)
        {
            return Equals(this, other);
        }

        public override bool Equals(object obj)
        {
            return obj is ServiceEndPoint && Equals(this, (ServiceEndPoint)obj);
        }

        public override int GetHashCode()
        {
            return (Host ?? "").GetHashCode() + Port;
        }

        public override string ToString()
        {
            return string.Format("{0}://{1}:{2}", Protocol, Host, Port);
        }
    }
}