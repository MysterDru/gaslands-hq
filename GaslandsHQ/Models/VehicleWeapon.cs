using System;
using PropertyChanged;

namespace GaslandsHQ.Models
{
    public class VehicleWeapon : NotifyPropertyChangedModel
    {
        [AlsoNotifyFor(nameof(Cost))]
        public Weapon Weapon { get; set; }

        [AlsoNotifyFor(nameof(Cost))]
        public string Facing { get; set; }

        public int Cost => this.Facing == "360" ? (Weapon?.Cost ?? 0) * 3 : Weapon?.Cost ?? 0;
    }
}
