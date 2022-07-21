namespace KleinEngine
{
    public interface IMediator
    {
        string name { get; }
        string state { get; }
        BaseView viewComponent { get; set; }
        void onRegister();
        void onUpdate();
        void onEnter();
        void onExit();
        void onRemove();
    }
}
