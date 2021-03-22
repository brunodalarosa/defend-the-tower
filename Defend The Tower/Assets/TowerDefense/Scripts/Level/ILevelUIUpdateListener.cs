namespace TowerDefense.Scripts.Level
{
    public interface ILevelUIUpdateListener
    {
        void UpdateBuildingEnergy(int available, int limit);
        void UpdatePortalLife(int portalLife);
    }
}