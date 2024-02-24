using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

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
        public float px = 0, py = 0, pz = 0, ry = 0;

        public void set(string name, float px, float py, float pz, float ry)
        {
            this.name = name;
            this.px = px;
            this.py = py;
            this.pz = pz;
            this.ry = ry;
        }
    }
    [Serializable]
    public class Roca : Pack
    {
        public float px = 0, py = 0, pz = 0;
        public int modelo = 0;
        public void set(float px, float py, float pz)
        {
            this.px = px;
            this.py = py;
            this.pz = pz;
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