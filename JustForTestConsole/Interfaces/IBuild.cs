namespace JustForTestConsole.Interfaces
{
    public interface IBuild
    {
        void Install(string deviceId);
        void Uninstall(string deviceId);
        void Download();
        void ClearData(string deviceId);
        void ForceStop(string deviceId);
        void CheckNewBuild();
    }
}
