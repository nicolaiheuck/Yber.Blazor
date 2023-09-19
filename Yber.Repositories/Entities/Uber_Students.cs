namespace Yber.Repositories.Entities
{
    public class Uber_Students 
    {
        public int Id { get; set; }
        public string Name_First { get; set; }
        public string Name_Last { get; set; }
        public string Street_Name { get; set; }
        public string Street_Number { get; set; }
        public string Longitude { get; set; }
        public string Lattitude { get; set; }
        public int Zipcode { get; set; }
        public bool Lift_Take { get; set; }
        public bool Lift_Give { get; set; }
        public decimal Lift_Distance { get; set; }
        
        // Navigation properties
        public Uber_Cities City { get; set; }
        public ICollection<Uber_Requests> Requests { get; set; }
    }
}