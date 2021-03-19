namespace TowerDefense.Scripts.Level
{
    public interface ILevelUIUpdateListener
    {
        void UpdateBuildingEnergyText(int available, int limit);
    }
}