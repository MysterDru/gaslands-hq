using System;
namespace GaslandsHQ.Models
{
    public class VehicleWeapon : NotifyPropertyChangedModel
    {
        public Weapon Weapon { get; set; }

        public string Facing { get; set; }
    }
}
