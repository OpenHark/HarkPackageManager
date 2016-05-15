using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;
using System.IO;
using System;

namespace System.IO
{
    public static class ExtendedStream
    {
        private static bool IsEndChar(char c)
        {
            return new char[]
                { '\t', '\r', '\n', ' ' }
                .Any(cx => cx == c);
        }
        
        public static char ReadChar(this Stream stream)
        {
            return (char)stream.ReadByte();
        }
        
        public static string ReadWord(this Stream stream)
        {
            bool wrapped = false;
            StringBuilder sb = new StringBuilder();
            
            try
            {
                char c;
                do
                {
                    c = stream.ReadChar();
                    if(wrapped)
                    {
                        if(c == '\"')
                        {
                            wrapped = sb.Length == 0 || sb.GetLastChar() != '\\';
                            if(!wrapped)
                                continue;
                        }
                    }
                    sb.Append(c);
                } while(wrapped || !IsEndChar(c));
            }
            catch(System.IO.IOException)
            { }
            
            return sb.ToString();
        }
        
        public static byte[] ReadBytes(this Stream stream)
        {
            using(MemoryStream ms = new MemoryStream())
            {
                try
                {
                    stream.CopyTo(ms);
                }
                catch
                { }
                
                return ms.ToArray();
            }
        }
        
        public static string ReadAllString(this Stream stream)
        {
            return stream.ReadBytes().GetString();
        }
        
        public static byte[] ReadUntil(this Stream stream, byte limitValue, bool included = false)
        {
            List<byte> data = new List<byte>();
            
            int value;
            while((value = stream.ReadByte()) != limitValue)
                data.Add((byte)value);
            
            if(included)
                data.Add(limitValue);
                
            return data.ToArray();
        }
        
        public static void Write(this Stream stream, byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }
        
        public static byte[] ReadWrapped(this Stream stream)
        {
            int size = stream.ReadInt();
            
            byte[] data = new byte[size];
            int index = 0;
            while((index += stream.Read(data, index, size - index)) < size)
                ;
            return data;
        }
        public static void WriteWrapped(this Stream stream, byte[] data)
        {
            stream.Write(data.Length);
            stream.Write(data);
        }
        
        public static void Write(this Stream stream, BigInteger value)
        {
            byte[] data = value.ToByteArray();
            stream.Write(data.Length);
            stream.Write(data);
        }
        public static BigInteger ReadBigInteger(this Stream stream)
        {
            int size = stream.ReadInt();
            
            List<byte> bytes = new List<byte>();
            for(uint i = 0; i < size; ++i)
                bytes.Add((byte)stream.ReadByte());
                
            return new BigInteger(bytes.ToArray());
        }
        
        public static void Write(this Stream stream, int value)
        {
            stream.Write(value.GetBytes());
        }
        public static int ReadInt(this Stream stream)
        {
            int value = 0;
            
            for(uint i = 0; i < 4; ++i)
            {
                value <<= 8;
                value += stream.ReadByte();
            }
            
            return value;
        }
        
        public static void Write(this Stream stream, string value)
        {
            byte[] data = value.GetBytes();
            stream.Write(data.Length);
            stream.Write(data);
        }
        public static string ReadString(this Stream stream)
        {
            int size = stream.ReadInt();
            
            StringBuilder sb = new StringBuilder();
            for(uint i = 0; i < size; ++i)
                sb.Append((char)stream.ReadByte());
                
            return sb.ToString();
        }
        
        public static void Write(this Stream stream, bool value)
        {
            stream.WriteByte((byte)(value ? 1 : 0));
        }
        public static bool ReadBool(this Stream stream)
        {
            return stream.ReadByte() == 1;
        }
        
        public static int? ReadIntNull(this Stream stream)
        {
            return null;
        }
        public static void Write(this Stream stream, int? value)
        {
            bool isNull = value == null;
            stream.Write(isNull);
            if(!isNull)
                stream.Write((int)value.Value);
        }
    }   
}