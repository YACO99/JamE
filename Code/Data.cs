using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Godot;
namespace Data
{
    [Serializable]
    public class Pack
    {
        public static int size=10240;
        public int id = 0;
        public static byte[] GetBytes(Pack p)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream m = new MemoryStream();
            bf.Serialize(m, p);
            return m.ToArray();
        }

        public static Pack SetBytes(byte[] b)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream m = new MemoryStream(b);
            return (Pack)bf.Deserialize(m);
        }
    }
    [Serializable]
    public class Player : Pack
    {
        public string name = "";
        public Vector3 pos = Vector3.Zero, vel = Vector3.Zero;
        public float ry = 0;

        public void set(string name, Vector3 pos, Vector3 vel, float ry)
        {
            this.name = name;
            this.pos = pos;
            this.vel = vel;
            this.ry = ry;
        }
    }
    [Serializable]
    public class Roca : Pack
    {
        public Vector3 pos = Vector3.Zero, vel=Vector3.Zero;
        public int modelo = 0;
        public void set(Vector3 pos, Vector3 vel)
        {
            this.pos = pos;
            this.vel = vel;
        }
    }
    [Serializable]
    public class Escena : Pack
    {
        public List<Player> players = new List<Player>();
        public List<Roca> rocas = new List<Roca>();
        public string Chat = "";
    }
    [Serializable]
    public class Chat : Pack
    {
        public string chat = "";
    }
}