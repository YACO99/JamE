using System.Runtime.Serialization.Formatters.Binary;

namespace Data
{
    [Serializable]
    public class Pack
    {
        public int id = 0;
        public string name = "";
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
        public int id = 0;
        public string name = "";
        public float px=0,py=0,pz=0,ry=0;
    }
}