namespace Kotoha.Plugin
{
    public interface IKotohaTalker
    {
        /// <summary>
        ///     Unique ID, I recommend roma-ji talker name.
        ///     Example: 琴葉茜 -> Akane, 結月ゆかり -> Yukari
        /// </summary>
        string Id { get; }

        string Name { get; }

        string Engine { get; }
    }
}