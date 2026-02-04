public class GameContext
{
    public GameContext(Player player, EnemySpawner spawner, OverlayMenu overlay, GameSound sound, RoundSet rounds, RoundCounter counter, ThirdPersonActions actions, PauseActions pauseAction, HUD hud) 
    {
        Player = player;
        Spawner = spawner;
        Overlay = overlay;
        Sound = sound;
        Rounds = rounds;
        Counter = counter;
        Actions = actions;
        HUD = hud;
    }

    public Player Player { get; private set; }
    public EnemySpawner Spawner { get; private set; }
    public OverlayMenu Overlay { get; private set; }
    public GameSound Sound { get; private set; }
    public RoundSet Rounds { get; private set; }
    public RoundCounter Counter { get; private set; }
    public ThirdPersonActions Actions { get; private set; }
    public PauseActions PauseAction { get; private set; }
    public HUD HUD { get; private set; }
}
