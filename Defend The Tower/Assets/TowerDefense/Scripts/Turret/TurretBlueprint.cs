using System;

namespace TowerDefense.Scripts.Turret
{
    [Serializable]
    public class TurretBlueprint
    {
        public TurretType _type;
        public int _energyCost;
        public TurretController _prefab;

        public TurretBlueprint(TurretType type, int energyCost)
        {
            _type = type;
            _energyCost = energyCost;
        }
    }
}