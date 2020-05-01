namespace Specter.Data.Models
{
    public class Sensor : Entity
    {
        public string Name { get; set; }
        public int Enabled { get; set; }
        public int Type { get; set; }
        public int Zone_Id { get; set; }
    }
}
