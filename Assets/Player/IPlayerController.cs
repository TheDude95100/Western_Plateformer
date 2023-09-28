public interface IPlayerController
{
    bool Falling { get; }
    bool isWalking { get; }
    bool Jumping { get; }
    bool Shooting { get; }
    bool TouchingWall { get; }

    bool IsJumping { get; }
    bool Dead { get; }
}