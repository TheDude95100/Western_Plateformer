public interface IPlayerController
{
    bool Dashing { get; }
    bool Dead { get; }
    bool Falling { get; }
    bool IsJumping { get; }
    bool isWalking { get; }
    bool Jumping { get; }
    bool Shooting { get; }
    bool TouchingWall { get; }

    bool Hurting { get; }
}